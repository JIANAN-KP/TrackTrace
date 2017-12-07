using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT_Match.model;
using TT_Match.tools;
using TT_Match.logic;


namespace TT_Match.logic
{
    public class Compare
    {
        public bool CompareSrcDes(Queue<KeyValuePair<string, string>> srcQueue, MagentaData magentaData,string sampleCode)
        {
            Queue<KeyValuePair<string, string>> checkQ = new Queue<KeyValuePair<string, string>>(srcQueue);
            Queue<KeyValuePair<string, string>> tempMatchQ = new Queue<KeyValuePair<string, string>>(srcQueue);
            Queue<KeyValuePair<string, string>> magentaQueue = magentaData.Queue;
            Queue<KeyValuePair<string, string>> tempMagentaQ = DequeueOneLine(magentaQueue);
            Queue<string> codeQueue = magentaData.SampleCodeQ;
            string code = codeQueue.Dequeue();
            /* check destination plate version */
            if(!CheckDataValid(checkQ))
            {
                FileProcessor.GiveLog("Export File Version Error");
                return false;
            }
            foreach (KeyValuePair<string, string> pair in tempMatchQ)
            {
                FileProcessor.GiveLog("Comparing  " + pair.Key + "    " + pair.Value);
            }
            bool result = MatchOneLine(CloneQueue(tempMatchQ), tempMagentaQ,code,sampleCode);
            if(!result)
            {
                while (!result)     //compare until all data used.
                {
                    if (magentaQueue.Count == 0)
                    {
                        FileProcessor.GiveLog("No more magenta data");
                        break;
                    }
                    else
                    {
                        tempMagentaQ = DequeueOneLine(magentaQueue);
                        code = codeQueue.Dequeue();
                        result = MatchOneLine(CloneQueue(tempMatchQ), tempMagentaQ, code, sampleCode);
                    }
                }
            }
            
            if (result == false)             
            {
                FileProcessor.GiveLog("Match Unsuccessfully  ");
            }
            return result;
        }

        public Queue<KeyValuePair<string, string>> DequeueOneLine(Queue<KeyValuePair<string, string>> orgQueue)
        {
            Queue<KeyValuePair<string, string>> queue = new Queue<KeyValuePair<string, string>>();
            int mark = 0;
            bool loop = true;
            while (loop && (orgQueue.Count != 0))
            {
                if ((orgQueue.First().Key.Equals(Constant.StrSrc)) && (mark == 0))
                {
                    KeyValuePair<string, string> pair = orgQueue.Dequeue();
                    queue.Enqueue(new KeyValuePair<string, string>(pair.Key, pair.Value.GetNumCode()));
                }
                else if (orgQueue.First().Key.Equals(Constant.StrDes))
                {
                    KeyValuePair<string, string> pair = orgQueue.Dequeue();
                    if(!FileProcessor.CheckCodeExist(queue,pair))
                    {
                        queue.Enqueue(new KeyValuePair<string, string>(pair.Key, pair.Value.GetNumCode()));
                    }else if(pair.Value.Equals(Constant.StrEmpty))
                    {
                        queue.Enqueue(new KeyValuePair<string, string>(pair.Key, Constant.StrEmpty));
                    }
                    mark = 1;
                }
                else //key equalto src whil mark =1 , next line start 
                {
                    loop = false;
                }
            }
            return queue;
        }

        /* delete V1 behind */
        public Queue<KeyValuePair<string,string>> RemoveVersionNum(Queue<KeyValuePair<string, string>> srcQueue)
        {
            Queue<KeyValuePair<string, string>> queue = new Queue<KeyValuePair<string, string>>();
            foreach(KeyValuePair<string,string> pair in srcQueue)
            {
                KeyValuePair<string, string> p = new KeyValuePair<string, string>(pair.Key, pair.Value.GetOrgCode().GetNumCode());
                queue.Enqueue(p);
            }
            return queue;
        }

        public Queue<KeyValuePair<string, string>> RemoveDuplicateDestination(Queue<KeyValuePair<string, string>> srcQueue)
        {
            Queue<KeyValuePair<string, string>> queue = new Queue<KeyValuePair<string, string>>();
            string numberCode = "";
            foreach (KeyValuePair<string, string> pair in srcQueue)
            {
                if(pair.Key.Equals(Constant.StrDes))
                {
                    if(numberCode.Equals(pair.Value.GetOrgCode().GetNumCode()))
                    {

                    }
                    else
                    {
                        queue.Enqueue(new KeyValuePair<string, string>(pair.Key, pair.Value));
                        numberCode = pair.Value.GetOrgCode().GetNumCode();
                    }
                }
                else
                {
                    queue.Enqueue(new KeyValuePair<string, string>(pair.Key, pair.Value));
                }
            }
            return queue;
        }

        public bool MatchOneLine(Queue<KeyValuePair<string, string>> srcQue, Queue<KeyValuePair<string, string>> desQueue,string code,string sampleCode)
        {
            bool flag = true;
            /* compare source and destination plates */
            while (desQueue.Count != 0)
            {
                if (srcQue.First().Equals(desQueue.First()))
                {
                    srcQue.Dequeue();
                    desQueue.Dequeue();
                }
                else if((srcQue.First().Key.Equals(Constant.StrSrc.ToLower()))&&(srcQue.First().Value.Equals(Constant.StrEmpty))&&(desQueue.First().Key.Equals(Constant.StrDes.ToLower())))
                {
                    /* source plates contains empty */
                    srcQue.Dequeue();
                }
                else
                {
                    flag = false;
                    break;
                }
            }
            if(flag == true)
            {
                if (!LeftEmpty(CloneQueue(srcQue)))
                {
                    flag = false;
                }
                /* if source and destination matched, then compare sample code */
                int codeNum = Convert.ToInt32(code);
                switch (sampleCode)
                {
                    case "96":
                        if (!(codeNum <= 96))
                        {
                            flag = false;
                            FileProcessor.GiveLog("Sample code not in correct scope");
                        }
                        break;
                    case "192":
                        if (!(codeNum > 96 && codeNum <= 192))
                        {
                            flag = false;
                            FileProcessor.GiveLog("Sample code not in correct scope");
                        }
                        break;
                    case "384":
                        if (!(codeNum > 192))
                        {
                            flag = false;
                            FileProcessor.GiveLog("Sample code not in correct scope");
                        }
                        break;
                    default:
                        break;
                }
            }            
            return flag;
        }

        private bool LeftEmpty(Queue<KeyValuePair<string, string>> queue)
        {
            bool flag = true;
            while(queue.Count>0)
            {
                KeyValuePair<string, string> pair = queue.Dequeue();
                if(!pair.Value.Equals(Constant.StrEmpty))
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        private Queue<KeyValuePair<string,string>> CloneQueue(Queue<KeyValuePair<string, string>> srcQ)
        {
            Queue<KeyValuePair<string, string>> desQ = new Queue<KeyValuePair<string, string>>();
            foreach(KeyValuePair<string,string> srcPair in srcQ)
            {
                desQ.Enqueue(new KeyValuePair<string, string>(srcPair.Key, srcPair.Value));
            }
            return desQ; 
        }

        private bool CheckDataValid(Queue<KeyValuePair<string,string>> srcQ)
        {
            bool flag = true;
            Queue<KeyValuePair<string, string>> desQ = new Queue<KeyValuePair<string, string>>(srcQ);
            Queue<KeyValuePair<string, string>> temQ = new Queue<KeyValuePair<string, string>>();
            for(int i = 0;i<desQ.Count;i++)
            {
                KeyValuePair<string, string> pair = desQ.Dequeue();
                if (pair.Key.Equals(Constant.StrDes))
                {
                    temQ.Enqueue(pair);   
                }
            }     
            if(temQ.Count>1)
            {
                KeyValuePair<string, string> pair1 = temQ.Dequeue();
                KeyValuePair<string, string> pair2 = temQ.Dequeue();
                string numCode1 = pair1.Value.GetNumCode();
                string numCode2 = pair2.Value.GetNumCode();
                string verCode1=  pair1.Value.GetVersionCode();
                string verCode2 = pair2.Value.GetVersionCode();
                if (numCode1.Equals(numCode2) && (Convert.ToInt32(verCode1).Equals(Convert.ToInt32(verCode2))))
                {
                    while (temQ.Count != 0)
                    {
                        pair1 = pair2;
                        pair2 = temQ.Dequeue();
                        numCode1 = pair1.Value.GetNumCode();
                        numCode2 = pair2.Value.GetNumCode();
                        verCode1 = pair1.Value.GetVersionCode();
                        verCode2 = pair2.Value.GetVersionCode();
                        if(numCode1.Equals(numCode2) && (Convert.ToInt32(verCode1)<(Convert.ToInt32(verCode2))))
                        {

                        }
                        else
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                else
                {
                    flag = false;
                }  
            }
            return flag;                                          
        }
    }
}


using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace MatchToSample
{
    static class BuildCombo
    {
        public static void Permute(ArrayList list, string[] strings, int start, int finish)
        {
            if (start == finish)
            {
                string[] newString = new string[strings.Length];
                strings.CopyTo(newString, 0);
                list.Add(newString);

            }
            else
            {
                for (int i = start; i <= finish; ++i)
                {
                    string temp = strings[start];
                    strings[start] = strings[i];
                    strings[i] = temp;

                    Permute(list, strings, start + 1, finish);

                    temp = strings[start];
                    strings[start] = strings[i];
                    strings[i] = temp;
                }
            }

        }

        public class Step
        {
            public string Questions;
            public string[] Options = null;
            public int AnswerIndex;
        }

        static void FindComb(Step[] steps, ArrayList comb, string ans, int iSteps)
        {
            foreach (string[] str in comb)
            {
                int ansIndex = -1;
                for (int i = 0; i < str.Length; ++i)
                {
                    if (ans.Equals(str[i]))
                    {
                        ansIndex = i;
                        break;
                    }
                }

                //Check whether this index is same as last two answer index
                bool bfound = false;
                for (int j = iSteps - 2; j <= iSteps - 1; j++)
                {
                    if (j >= 0 && steps[j] != null)
                    {
                        if (steps[j].AnswerIndex == ansIndex)
                        {
                            bfound = true;
                            break;
                        }
                    }
                }

                if (bfound)
                    continue;

                //Check wheter the combination is present in step array
                bfound = false;
                for (int j = iSteps - 2; j <= iSteps - 1; j++)
                {
                    if (j >= 0 && steps[j] != null)
                    {
                        bool allSame = true;

                        for (int k = 0; k < str.Length; k++)
                        {
                            if (str[k] != steps[j].Options[k])
                            {
                                allSame = false;
                                break;
                            }
                        }

                        if (allSame)
                        {
                            bfound = true;
                            break;
                        }
                    }
                }

                if (bfound)
                    continue;


                //Got the string
                steps[iSteps] = new Step();
                steps[iSteps].AnswerIndex = ansIndex;
                steps[iSteps].Options = str;
                steps[iSteps].Questions = "Find " + ans;
                return;

            }

            return;
        }

        public static  Step[] FormSteps(string[] Objects, int iTrials)
        {

            Step[] steps = new Step[iTrials];
            ArrayList list = new ArrayList();
            Permute(list, Objects, 0, Objects.Length - 1);

            int j = -1;
            for (int i = 0; i < iTrials; i++)
            {
                if (j == Objects.Length - 1)
                {
                    j = 0;
                }
                else
                {
                    j++;
                }
                FindComb(steps, list, Objects[j], i);
            }
            return steps;

        }
    }
}

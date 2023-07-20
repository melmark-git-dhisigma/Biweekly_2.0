using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text;

public class clsMathToSamples
{
    public clsMathToSamples()
    {

    }
    public static void Permute(ArrayList list, string[] strings, int start, int finish, int iTrials)
    {
        if (start == finish)
        {
            string[] newString = new string[strings.Length];
            strings.CopyTo(newString, 0);
            newString = Shuffle(newString);
            list.Add(newString);

        }
        else if (list.Count > iTrials)
        {
            return;
        }
        else
        {
            for (int i = start; i <= finish; ++i)
            {
                string temp = strings[start];
                strings[start] = strings[i];
                strings[i] = temp;

                Permute(list, strings, start + 1, finish, iTrials);

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
        public string TrialText;
        public Step MakeCopy()
        {
            Step newStep = new Step();
            newStep.AnswerIndex = this.AnswerIndex;
            newStep.Options = this.Options;
            newStep.Questions = this.Questions;
            newStep.TrialText = this.TrialText;
            return newStep;
        }
    }

    static void FindComb(Step[] steps, ArrayList comb, string ans, int iSteps, bool bRandom, bool bNoRule = false)
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

            if (!bNoRule)
            {
                ////Check whether there are matching index with previous step
                bool bfound = false;
                //if (iSteps > 0)
                //{
                //    for (int k = 0; k < str.Length; k++)
                //    {
                //        if (str[k] == steps[iSteps - 1].Options[k])
                //        {
                //            bfound = true;
                //            break;
                //        }

                //    }
                //    if (bfound)
                //        continue;
                //}

                //###  Rule
                if (bRandom)
                {
                    //Check whether this index is same as last two answer index
                    bfound = false;
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
                }
                //###  Rule

            }
            //Check wheter the combination is present in step array
            bool bfoundDup = false;
            //for (int j = iSteps - 2; j <= iSteps - 1; j++)
            for (int j = 0; j < steps.Length; j++)
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
                        bfoundDup = true;
                        break;
                    }
                }
            }

            if (bfoundDup)
                continue;


            //Got the string
            steps[iSteps] = new Step();
            steps[iSteps].AnswerIndex = ansIndex;
            steps[iSteps].Options = str;
            //steps[iSteps].Questions = "Find " + ans;
            steps[iSteps].Questions = ans;

            steps[iSteps].TrialText = "";
            foreach (string strTemp in str)
            {
                if (string.IsNullOrEmpty(steps[iSteps].TrialText))
                {
                    steps[iSteps].TrialText = "[" + strTemp;
                }
                else
                {
                    steps[iSteps].TrialText += ", " + strTemp;
                }
            }
            steps[iSteps].TrialText += "]";
            steps[iSteps].TrialText = steps[iSteps].Questions + ":  " + steps[iSteps].TrialText;
            return;

        }
        //Combination Not found .  Randomize elements...
        string[] str1 = (string[])comb[0];


        ArrayList anslist = new ArrayList();

        for (int j = iSteps - 2; j <= iSteps - 1; j++)
        {
            if (j >= 0 && steps[j] != null)
            {
                anslist.Add(steps[j].AnswerIndex);

            }
        }

        str1 = Shuffle(str1);
        int ansIndex1 = -1;
        for (int i = 0; i < str1.Length; ++i)
        {
            if (ans.Equals(str1[i]))
            {
                ansIndex1 = i;
                break;
            }
        }
        int newIndex = FindNewAnsIndex(anslist, str1.Length);
        string temp = str1[ansIndex1];
        str1[ansIndex1] = str1[newIndex];
        str1[newIndex] = temp;

        steps[iSteps] = new Step();
        steps[iSteps].AnswerIndex = newIndex;
        steps[iSteps].Options = str1;
        //steps[iSteps].Questions = "Find " + ans;
        steps[iSteps].Questions = ans;

        steps[iSteps].TrialText = "";
        foreach (string strTemp in str1)
        {
            if (string.IsNullOrEmpty(steps[iSteps].TrialText))
            {
                steps[iSteps].TrialText = "[" + strTemp;
            }
            else
            {
                steps[iSteps].TrialText += ", " + strTemp;
            }
        }
        steps[iSteps].TrialText += "]";
        steps[iSteps].TrialText = steps[iSteps].Questions + ":  " + steps[iSteps].TrialText;


        return;
    }

    public static Step[] FormSteps(string[] Objects, int iTrials)
    {
        Objects = Shuffle(Objects);
        Step[] steps = new Step[iTrials];
        ArrayList list = new ArrayList();
        Permute(list, Objects, 0, Objects.Length - 1, iTrials);

        int j = -1;
        bool bRandomRule = true;
        if (Objects.Length <= 3)
            bRandomRule = false;
        for (int i = 0; i < iTrials; i++)
        {
            if (i > Objects.Length - 1)
            {
                steps[i] = steps[i - Objects.Length].MakeCopy();
                //foreach (Step step in steps)
                //{
                //    if (step !=null && step.Questions == "Find " + AnsList[i])
                //    {
                //        steps[i] = step.MakeCopy();
                //        break;
                //    }

                //}
            }
            else
            {
                FindComb(steps, list, Objects[i], i, bRandomRule);
            }
        }
        return steps;

    }
    private static string[] Shuffle(string[] Objects)
    {
        Random rand = new Random();

        for (int i = 0; i < Objects.Length; i++)
        {
            System.Threading.Thread.Sleep(1);
            int rVal = rand.Next(Objects.Length - 1);

            string temp = Objects[i];
            Objects[i] = Objects[rVal];
            Objects[rVal] = temp;

        }

        return Objects;
    }
    private static ArrayList ShuffleList(ArrayList Objects)
    {
        Random rand = new Random();

        for (int i = 0; i < Objects.Count; i++)
        {
            int rVal = rand.Next(Objects.Count - 1);

            object temp = Objects[i];
            Objects[i] = Objects[rVal];
            Objects[rVal] = temp;

        }

        return Objects;
    }
    public static Step[] ShuffleSteps(Step[] steps, Step[] stepsCopy, string[] Objects)
    {
        ClsErrorLog clError = new ClsErrorLog();
        Random rand = new Random();
        ArrayList randpos=new ArrayList();
        Step[] RandomSteps = new Step[stepsCopy.Length];
        int tempVal = 0;
        for (int i = 0; i < steps.Length; i++)
        {
            int randomLen = randpos.Count;
            int rVal = 0;
            int insert = 0;
            System.Threading.Thread.Sleep(1);
            rVal = rand.Next(steps.Length);
            if (i == 0)
            {
                randpos.Add(rVal);
            }
            else
            {
                if ((RandomSteps[i - 1].Options[0] != stepsCopy[rVal].Options[0]) && !randpos.Contains(rVal) && (RandomSteps[i - 1].Questions != stepsCopy[rVal].Questions))
                {
                    randpos.Add(rVal);
                }
                else
                {
                    int count = 0;
                    do
                    {
                        System.Threading.Thread.Sleep(1);
                        rVal = rand.Next(steps.Length);
                        if ((RandomSteps[i - 1].Options[0] != stepsCopy[rVal].Options[0] || (RandomSteps[i - 1].Questions != stepsCopy[rVal].Questions)) && !randpos.Contains(rVal))
                        {
                            randpos.Add(rVal);
                            break;
                        }
                        else if (i == steps.Length - 1 && !randpos.Contains(rVal))
                        {
                            Step temp = RandomSteps[i];
                            for (int j = 0; j < stepsCopy.Length; j++)
                            {
                                tempVal = 0;
                                if (j == 0)
                                {
                                    if (stepsCopy[rVal].Options[0] == RandomSteps[j].Options[0] || stepsCopy[rVal].Options[0] == RandomSteps[j + 1].Options[0] || stepsCopy[rVal].Questions == RandomSteps[j].Questions || stepsCopy[rVal].Questions == RandomSteps[j + 1].Questions)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (RandomSteps[j].Options[0] != RandomSteps[i - 1].Options[0] || RandomSteps[j].Questions != RandomSteps[i - 1].Questions)
                                        {
                                            temp = RandomSteps[j];
                                            RandomSteps[j] = stepsCopy[rVal];
                                            tempVal = 1;
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                                else if (j == stepsCopy.Length - 1)
                                {
                                    if (RandomSteps[j] != null)
                                    {
                                        if (stepsCopy[rVal].Options[0] == RandomSteps[j - 1].Options[0] || stepsCopy[rVal].Options[0] == RandomSteps[j].Options[0] || stepsCopy[rVal].Questions == RandomSteps[j - 1].Questions || stepsCopy[rVal].Questions == RandomSteps[j].Questions)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (RandomSteps[j].Options[0] != RandomSteps[i - 1].Options[0] || RandomSteps[j].Questions != RandomSteps[i - 1].Questions)
                                            {
                                                temp = RandomSteps[j];
                                                RandomSteps[j] = stepsCopy[rVal];
                                                tempVal = 1;
                                                break;
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    else
                                        continue;
                                }
                                else
                                {
                                    if (RandomSteps[j] != null && RandomSteps[j + 1] != null)
                                    {
                                        if (stepsCopy[rVal].Options[0] == RandomSteps[j - 1].Options[0] || stepsCopy[rVal].Options[0] == RandomSteps[j].Options[0] || stepsCopy[rVal].Options[0] == RandomSteps[j + 1].Options[0] || stepsCopy[rVal].Questions == RandomSteps[j - 1].Questions || stepsCopy[rVal].Questions == RandomSteps[j].Questions || stepsCopy[rVal].Questions == RandomSteps[j + 1].Questions)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (RandomSteps[j].Options[0] != RandomSteps[i - 1].Options[0] || RandomSteps[j].Questions != RandomSteps[i - 1].Questions)
                                            {
                                                temp = RandomSteps[j];
                                                RandomSteps[j] = stepsCopy[rVal];
                                                tempVal = 1;
                                                break;
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    else
                                        continue;
                                }
                            }



                            if (tempVal == 1)
                            {
                                if (!(RandomSteps[i - 1].Options[0] == temp.Options[0]) || !(RandomSteps[i - 1].Questions == temp.Questions))// || !(steps[i].Options[0] == temp.Options[0]))
                                {
                                    RandomSteps[i] = temp;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }



                        }
                        else
                        {
                            count++;
                            if (count > (steps.Length * 10))
                            {
                                return new Step[0];
                                //break;
                            }
                            continue;
                        }

                    }
                    while (randomLen == randpos.Count || randpos.Count == stepsCopy.Length);
                }
            }
            if (tempVal != 1)
            {
                RandomSteps[i] = stepsCopy[rVal];
            }
        }
        //clError.WriteToLog("Result steps:");
        //clError.WriteToLog("");
        //for (int i = 0; i < steps.Length; i++)
        //{
        //    string result = "";
        //    foreach (var item in steps[i].Options)
        //    {
        //        result += item+",";
        //    }
        //    result.Substring(0,result.Length - 1);
        //    clError.WriteToLog(i+1+")"+result);
        //}
        //clError.WriteToLog("");
        return RandomSteps;
    }
    public static Step[] FormStepsInOrder(string[] Objects, int iTrials)
    {

        Objects = Shuffle(Objects);
        Step[] steps = new Step[iTrials];
        ArrayList list = new ArrayList();
        string[] newString = new string[Objects.Length];
        Objects.CopyTo(newString, 0);
        list.Add(newString);
        for (int k = 1; k < iTrials; k++)
        {
            string last = Objects[0];
            for (int l = 0; l < Objects.Length - 1; l++)
            {
                string temp = Objects[l + 1];
                Objects[l + 1] = last;
                last = temp;
            }
            Objects[0] = last;
            string[] newStringTemp = new string[Objects.Length];
            Objects.CopyTo(newStringTemp, 0);
            list.Add(newStringTemp);
        }

        int j = -1;
        bool bNoRule = false;

        for (int i = 0; i < iTrials; i++)
        {

            if (i > Objects.Length - 1)
            {
                steps[i] = steps[i - Objects.Length].MakeCopy();

            }
            else
                FindComb(steps, list, Objects[i], i, false, bNoRule);
        }
        return steps;
    }
    public static Step[] FormStepsWithAns(string[] Objects, int iTrials, string[] AnsList)
    {

        Objects = Shuffle(Objects);


        Step[] steps = new Step[iTrials];
        ArrayList list = new ArrayList();
        ArrayList StepList = new ArrayList();
        Permute(list, Objects, 0, Objects.Length - 1, iTrials);

        list = ShuffleList(list);
        int j = 0;
        bool bRandomRule = true;
        if (Objects.Length <= 3)
            bRandomRule = false;
        for (int i = 0; i < AnsList.Length; i++)
        {

            if (j > Objects.Length - 1)
            {
                j = 0;
                steps = new Step[iTrials];
                list = ShuffleList(list);
            }

            FindComb(steps, list, AnsList[i], j, bRandomRule);
            StepList.Add(steps[j].MakeCopy());
            j++;
        }
        Step[] stepsTemp = new Step[StepList.Count];
        j = 0;
        foreach (Step stp in StepList)
        {
            stepsTemp[j++] = stp;
        }
        return stepsTemp;

    }

    public static Step[] FormStepsWithAnsNew(string[] Objects, int iTrials, string[] AnsList)
    {

        Objects = Shuffle(Objects);

        Step[] steps = new Step[iTrials];
        ArrayList list = new ArrayList();
        ArrayList StepList = new ArrayList();
        //Permute(list, Objects, 0, Objects.Length - 1, iTrials);

        string[] newString = new string[Objects.Length];
        Objects.CopyTo(newString, 0);
        list.Add(newString);
        for (int k = 1; k < iTrials; k++)
        {
            string last = Objects[0];
            for (int l = 0; l < Objects.Length - 1; l++)
            {
                string temp = Objects[l + 1];
                Objects[l + 1] = last;
                last = temp;
            }
            Objects[0] = last;
            string[] newStringTemp = new string[Objects.Length];
            Objects.CopyTo(newStringTemp, 0);
            list.Add(newStringTemp);
        }

        list = ShuffleList(list);
        int j = 0;
        bool bRandomRule = true;
        if (Objects.Length <= 3)
            bRandomRule = false;
        for (int i = 0; i < AnsList.Length; i++)
        {

            if (j > Objects.Length - 1)
            {
                j = 0;
                steps = new Step[iTrials];
                list = ShuffleList(list);
            }

            FindComb(steps, list, AnsList[i], j, bRandomRule);
            StepList.Add(steps[j].MakeCopy());
            j++;
        }
        Step[] stepsTemp = new Step[StepList.Count];
        Step[] rtrnstepsTemp = new Step[StepList.Count];
        j = 0;
        foreach (Step stp in StepList)
        {
            stepsTemp[j++] = stp;
        }
        do
        {
            rtrnstepsTemp = ShuffleSteps(stepsTemp, stepsTemp, Objects);
        } while (rtrnstepsTemp.Length == 0);
        return rtrnstepsTemp;

    }

    public static Step[] FormStepsInOrderWithAns(string[] Objects, int iTrials, string[] AnsList)
    {


        Objects = Shuffle(Objects);

        Step[] steps = new Step[iTrials];
        ArrayList list = new ArrayList();
        string[] newString = new string[Objects.Length];
        Objects.CopyTo(newString, 0);
        list.Add(newString);
        for (int k = 1; k < iTrials; k++)
        {
            string last = Objects[0];
            for (int l = 0; l < Objects.Length - 1; l++)
            {
                string temp = Objects[l + 1];
                Objects[l + 1] = last;
                last = temp;
            }
            Objects[0] = last;
            string[] newStringTemp = new string[Objects.Length];
            Objects.CopyTo(newStringTemp, 0);
            list.Add(newStringTemp);
        }

        bool bNoRule = false;
        //for (int i = 0; i < AnsList.Length; i++)
        //{
        //    if (i > Objects.Length - 1)
        //        bNoRule = true;
        //    FindComb(steps, list, AnsList[i], i, false, bNoRule);
        //}
        for (int i = 0; i < AnsList.Length; i++)
        {
            if (i > Objects.Length - 1)
            {
                steps[i] = steps[i - Objects.Length].MakeCopy();
                //foreach (Step step in steps)
                //{
                //    if (step !=null && step.Questions == "Find " + AnsList[i])
                //    {
                //        steps[i] = step.MakeCopy();
                //        break;
                //    }

                //}
            }
            else
                FindComb(steps, list, AnsList[i], i, false, bNoRule);
        }
        return steps;
    }
    public static string GetQuestion(string question)
    {
        question = question.Replace("Find ", "");
        question = question.Substring(0, question.IndexOf(": "));
        //question = question.Replace(" ", "");
        return question;
    }
    public static string[] GetAnsList(string question)
    {

        question = question.Substring(question.IndexOf("[") + 1);
        question = question.Replace("]", "").Trim();
        //question = question.Replace(" ", "");
        return question.Split(',');

    }
    static int FindNewAnsIndex(ArrayList PrevIndex, int length)
    {
        Dictionary<int, int> DictPrevIndex = new Dictionary<int, int>();
        foreach (int index in PrevIndex)
        {
			if(!DictPrevIndex.ContainsKey(index))
                DictPrevIndex.Add(index, index);
        }

        int newIndex = 0;
        Random rand = new Random();
        int icount = 0;

        while (icount++ < 1000)
        {
            int rVal = rand.Next(length);
            if (!DictPrevIndex.ContainsKey(rVal))
                return rVal;
        }

        for (int i = 0; i < length; i++)
        {
            if (!DictPrevIndex.ContainsKey(i))
                return i;
        }
        if (PrevIndex.Count > 0) return (int)PrevIndex[0];
        else return 0;
    }
    static int FindNewAnsIndexOLD(ArrayList PrevIndex, int length)
    {
        int newIndex = 0;
        Random rand = new Random();
        int icount = 0;

        while (icount < 1000)
        {
            int rVal = rand.Next(length - 1);
            bool dupe = false;
            foreach (int index in PrevIndex)
            {
                if (rVal == index)
                {
                    dupe = true;
                    break;
                }
            }
            if (dupe == false) return rVal;
        }

        for (int i = 0; i < length; i++)
        {
            bool dupe = false;
            foreach (int index in PrevIndex)
            {
                if (i == index)
                {
                    dupe = true;
                    break;
                }
            }
            if (dupe == false) return i;
        }
        if (PrevIndex.Count > 0) return (int)PrevIndex[0];
        else return 0;
    }

}

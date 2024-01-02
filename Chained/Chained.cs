using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

public enum MoveType
{
    PromptMoveup,
    PromptMoveDown,
    StepMoveUp,
    StepMoveDown,
    SetMoveUp,
    SetMoveDown
}

namespace Chained
{

    public class Step
    {
        public string Score = "";
        public string Prompt = "";
        public int Duration;
        public bool Mistrial = false;
        public float AccuracyPercentage = 0;
        public Dictionary<string, float> PromptAverages = new Dictionary<string, float>();
        public Dictionary<string, float> PromptCount = new Dictionary<string, float>();

    }
    public class Session
    {
        public Step[] Steps = null;
        public float AccuracyPercentageAVG = 0;
        public float IndependentPercentageAVG = 0;
        public float AccuracyCount = 0;
        public float IndependenceCount = 0;
        public int GetNoScoredSessionCount()
        {
            int count = 0;
            foreach (var Step in Steps)
            {
                if (Step.Score != "" && Step.Score != "0")
                {
                    count++;
                }
            }
            return count;
        }

        public int GetNoScoredFreqDurSessionCount()
        {
            int count = 0;
            foreach (var Step in Steps)
            {
                if (Step.Score != "" && Step.Score != "00:00:00" && Step.Score != "-1" && Step.Score != "0")
                {
                    count++;
                }
            }
            return count;
        }
    }
    public class TrialAvg
    {
        public float AVGScore;
        public float AVGPrompt;
        public float AVGIndependence;
        public float AVGduration;
        public float Correct;
        public float Incorrect;
        public Dictionary<string, float> PromptAverages = new Dictionary<string, float>();

    }
    public class MoveCriteria
    {
        public int BarCondition;
        public int TotalTrial;
        public int SuccessNeeded;
        public bool ConsecutiveSuccess = false;
        public bool bIOAReqd = false;
        public bool bMultiTchr = false;
        public bool ConsecutiveAverage = false; //--- [New Criteria] May 2020 ---//
        public int ConsecutiveAverageValue = 0; //--- [New Criteria] May 2020 ---//

    }
    public class MoveBackCriteria
    {
        public int BarCondition;
        public int TotalTrial;
        public int FailureNeeded;
        public bool ConsecutiveFailures = false;
        public bool bIOAReqd = false;
        public bool bMultiTchr = false;
        public bool ConsecutiveAverageFailure = false; //--- [New Criteria] May 2020 ---//
        public int ConsecutiveAverageFailValue = 0; //--- [New Criteria] May 2020 ---//

    }
    public class InputData
    {
        public bool PromptHirecharchy = false;
        public bool MultiSet = false;
        public bool IOARequired = false;
        public bool MultiTeacherRequired = false;
        public bool IncludeMistrials = false;
        public TrialAvg[] TrialsData = null;
        public string[] PromptsUsed = null;
        public string[] NoPromptsUsed = null;
        public string sCurrentLessonPrompt = "";
        public string CurrentPrompt = "";
        public string TargetPrompt = "";
        public int CurrentSet;
        public int CurrentStep;
        public int TotalSets;
        public Session[] Sessions = null;
        public int SessionCount = 0;
        public int StepCount = 0;
        public bool TotalTaskMode = false;
        public string[] StepPrompts = null;
        public string CorrectResp = "";
        public int promptUp = 0;
        public int promptDown = 0;
        public string promptType = "";
        public int next;
        public int marked;        
        public bool multiMeasure = false;
        public int FreDurMoveStat = 0;
        //Step Move Criteria
        public MoveCriteria StepPercentAccuracy = new MoveCriteria();
        public MoveCriteria StepPercentIndependence = new MoveCriteria();


        //Step Move Back Criteria
        public MoveBackCriteria StepMoveBackPercentAccuracy = new MoveBackCriteria();
        public MoveBackCriteria StepMoveBackPercentIndependence = new MoveBackCriteria();

        //Set Move Criteria
        public MoveCriteria PercentAccuracy = new MoveCriteria();
        public MoveCriteria PercentIndependence = new MoveCriteria();
        public MoveCriteria PercentOpportunity = new MoveCriteria();
        public MoveCriteria Rate = new MoveCriteria();

        //Set Move Back Criteria
        public MoveBackCriteria MoveBackPercentAccuracy = new MoveBackCriteria();
        public MoveBackCriteria MoveBackPercentIndependence = new MoveBackCriteria();

        //Prompt Move Criteria
        public MoveCriteria PromptPercentAccuracy = new MoveCriteria();
        public MoveCriteria PromptPercentIndependence = new MoveCriteria();

        //Prompt Move Back Criteria
        public MoveBackCriteria MoveBackPromptPercentAccuracy = new MoveBackCriteria();
        public MoveBackCriteria MoveBackPromptPercentIndependence = new MoveBackCriteria();

        //Custon Column Move Criteria
        public MoveCriteria CustomPercent = new MoveCriteria();
        //public MoveCriteria CustomPercentIndependence = new MoveCriteria();

        //Custon Column Move Back Criteria
        public MoveBackCriteria MoveBackCustom = new MoveBackCriteria();
        //public MoveBackCriteria MoveBackCustomPercentIndependence = new MoveBackCriteria();

        //Duration Column Move Criteria
        public MoveCriteria AvgDurationMoveUp = new MoveCriteria();
        public MoveCriteria StepAvgDurationMoveUp = new MoveCriteria();
        public MoveCriteria PromptAvgDurationMoveUp = new MoveCriteria();
        public MoveCriteria TotalDurationMoveUp = new MoveCriteria();
        public MoveCriteria StepTotalDurationMoveUp = new MoveCriteria();
        public MoveCriteria PromptTotalDurationMoveUp = new MoveCriteria();
        //public MoveCriteria CustomPercentIndependence = new MoveCriteria();

        //Duration Column Move Back Criteria
        public MoveBackCriteria AvgDurationMoveDown = new MoveBackCriteria();
        public MoveBackCriteria StepAvgDurationMoveDown = new MoveBackCriteria();
        public MoveBackCriteria PromptAvgDurationMoveDown = new MoveBackCriteria();
        public MoveBackCriteria TotalDurationMoveDown = new MoveBackCriteria();
        public MoveBackCriteria StepTotalDurationMoveDown = new MoveBackCriteria();
        public MoveBackCriteria PromptTotalDurationMoveDown = new MoveBackCriteria();
        //public MoveBackCriteria MoveBackCustomPercentIndependence = new MoveBackCriteria();

        //Frequency Column Move Criteria
        public MoveCriteria FrequencyMoveUp = new MoveCriteria();
        public MoveCriteria StepFrequencyMoveUp = new MoveCriteria();
        public MoveCriteria PromptFrequencyMoveUp = new MoveCriteria();
        //public MoveCriteria CustomPercentIndependence = new MoveCriteria();

        //Frequency Column Move Back Criteria
        public MoveBackCriteria FrequencyMoveDown = new MoveBackCriteria();
        public MoveBackCriteria StepFrequencyMoveDown = new MoveBackCriteria();
        public MoveBackCriteria PromptFrequencyMoveDown = new MoveBackCriteria();
        //public MoveBackCriteria MoveBackCustomPercentIndependence = new MoveBackCriteria();


        //Learned Step Move Up Criteria
        public MoveCriteria LearnedStepMoveUp = new MoveCriteria();
        //Learned Step Move Back Criteria
        public MoveBackCriteria LearnedStepMoveBack = new MoveBackCriteria();


        //Learned Step Move Up Criteria for Prompt
        public MoveCriteria PromptLearnedStepMoveUp = new MoveCriteria();
        //Learned Step Move Back Criteria for prompt
        public MoveBackCriteria PromptLearnedStepMoveBack = new MoveBackCriteria();

        //Learned Step Move Up Criteria for Set
        public MoveCriteria SetLearnedStepMoveUp = new MoveCriteria();
        //Learned Step Move Back Criteria for set
        public MoveBackCriteria SetLearnedStepMoveBack = new MoveBackCriteria();

        //Exclude Current Step Move Up Criteria
        public MoveCriteria ExcludeCrntStepMoveUp = new MoveCriteria();
        //Exclude Current Step Move Back Criteria
        public MoveBackCriteria ExcludeCrntStepMoveBack = new MoveBackCriteria();

        //Exclude Current Step Move Up Criteria for prompt
        public MoveCriteria PromptExcludeCrntStepMoveUp = new MoveCriteria();
        //Exclude Current Step Move Back Criteria for prompt
        public MoveBackCriteria PromptExcludeCrntStepMoveBack = new MoveBackCriteria();

        //Exclude Current Set Move Up Criteria for set
        public MoveCriteria SetExcludeCrntStepMoveUp = new MoveCriteria();
        //Exclude Current Set Move Back Criteria for set
        public MoveBackCriteria SetExcludeCrntStepMoveBack = new MoveBackCriteria();



        //Total correct for prompt moveup
        public MoveCriteria PromptTotalCorrectMoveUp = new MoveCriteria();

        //Total Incorrect for prompt movedown
        public MoveBackCriteria PromptTotalIncorrectMoveBack = new MoveBackCriteria();
        public MoveBackCriteria PromptTotalCorrectMoveBack = new MoveBackCriteria();

        //Total Correct for Step moveup
        public MoveCriteria StepTotalCorrectMoveUp = new MoveCriteria();

        //Total Incorrect for Step movedown
        public MoveBackCriteria StepTotalIncorrectMoveBack = new MoveBackCriteria();
        public MoveBackCriteria StepTotalCorrectMoveBack = new MoveBackCriteria();

        //Total correct for Set moveup
        public MoveCriteria SetTotalCorrectMoveUp = new MoveCriteria();

        //Total Incorrect for Set movedown
        public MoveBackCriteria SetTotalIncorrectMoveBack = new MoveBackCriteria();
        public MoveBackCriteria SetTotalCorrectMoveBack = new MoveBackCriteria();

        //prompt percent indepedent of all steps 
        public MoveCriteria PromptPercentAllIndependence = new MoveCriteria();
        public MoveBackCriteria MoveBackPromptPercentAllIndependence = new MoveBackCriteria();

        //step percent indepedent of all steps 
        public MoveCriteria StepPercentAllIndependence = new MoveCriteria();
        public MoveBackCriteria StepMoveBackPercentAllIndependence = new MoveBackCriteria();

        //set percent indepedent of all steps 
        public MoveCriteria PercentAllIndependence = new MoveCriteria();
        public MoveBackCriteria MoveBackPercentAllIndependence = new MoveBackCriteria();


        //Coded added to check if the selected colum have the criteria for any moveup or movedown.  Arun.
        public bool IsInfluencedBy(MoveType moveType)
        {
            bool UseForDecission = false;
            switch (moveType)
            {
                case MoveType.PromptMoveup:
                    UseForDecission = (PromptPercentAccuracy.TotalTrial > 0) || (PromptPercentIndependence.TotalTrial > 0) || (PromptLearnedStepMoveUp.TotalTrial > 0) || (PromptTotalCorrectMoveUp.TotalTrial > 0 || PromptExcludeCrntStepMoveUp.TotalTrial > 0 || PromptFrequencyMoveUp.TotalTrial > 0 || PromptAvgDurationMoveUp.TotalTrial > 0 || PromptTotalDurationMoveUp.TotalTrial > 0);
                    break;
                case MoveType.PromptMoveDown:
                    UseForDecission = (MoveBackPromptPercentAccuracy.TotalTrial > 0) || (MoveBackPromptPercentIndependence.TotalTrial > 0) || (PromptLearnedStepMoveBack.TotalTrial > 0) || (PromptTotalIncorrectMoveBack.TotalTrial > 0) || (PromptTotalCorrectMoveBack.TotalTrial > 0 || MoveBackPromptPercentAllIndependence.TotalTrial > 0 || PromptFrequencyMoveDown.TotalTrial > 0 || PromptAvgDurationMoveDown.TotalTrial > 0 || PromptTotalDurationMoveDown.TotalTrial > 0);
                    break;
                case MoveType.StepMoveUp:
                    UseForDecission = (StepPercentAccuracy.TotalTrial > 0) || (StepPercentIndependence.TotalTrial > 0) || (LearnedStepMoveUp.TotalTrial > 0) || (StepTotalCorrectMoveUp.TotalTrial > 0 || ExcludeCrntStepMoveUp.TotalTrial > 0 || StepFrequencyMoveUp.TotalTrial > 0 || StepAvgDurationMoveUp.TotalTrial > 0 || StepTotalDurationMoveUp.TotalTrial > 0);
                    break;
                case MoveType.StepMoveDown:
                    UseForDecission = (StepMoveBackPercentAccuracy.TotalTrial > 0) || (StepMoveBackPercentIndependence.TotalTrial > 0) || (LearnedStepMoveBack.TotalTrial > 0) || (StepTotalIncorrectMoveBack.TotalTrial > 0) || (StepTotalCorrectMoveBack.TotalTrial > 0 || ExcludeCrntStepMoveBack.TotalTrial > 0 || StepMoveBackPercentAllIndependence.TotalTrial > 0 || StepFrequencyMoveDown.TotalTrial > 0 || StepAvgDurationMoveDown.TotalTrial > 0 || StepTotalDurationMoveDown.TotalTrial > 0);
                    break;
                case MoveType.SetMoveUp:
                    UseForDecission = (PercentAccuracy.TotalTrial > 0) || (PercentIndependence.TotalTrial > 0) || (SetLearnedStepMoveUp.TotalTrial > 0) || (SetTotalCorrectMoveUp.TotalTrial > 0 || SetExcludeCrntStepMoveUp.TotalTrial > 0 || FrequencyMoveUp.TotalTrial > 0 || AvgDurationMoveUp.TotalTrial > 0 || TotalDurationMoveUp.TotalTrial > 0 );
                    break;
                case MoveType.SetMoveDown:
                    UseForDecission = (MoveBackPercentAccuracy.TotalTrial > 0) || (MoveBackPercentIndependence.TotalTrial > 0) || (SetLearnedStepMoveBack.TotalTrial > 0) || (SetTotalIncorrectMoveBack.TotalTrial > 0) || (SetTotalCorrectMoveBack.TotalTrial > 0 || SetExcludeCrntStepMoveBack.TotalTrial > 0 || MoveBackPercentAllIndependence.TotalTrial > 0 || FrequencyMoveDown.TotalTrial > 0 || AvgDurationMoveDown.TotalTrial > 0 || TotalDurationMoveDown.TotalTrial > 0);
                    break;
                default:
                    break;

            }

            return UseForDecission;
        }


        static int ToInt(string sValue)
        {
            if (string.IsNullOrEmpty(sValue.Trim()))
            {
                return 0;
            }
            else return int.Parse(sValue);
        }
        public void SetFlags(string sPromptHirecharchy, string sMultiSet, string sIOARequired, string sMultiTeacherRequired, string sIncludeMistrials)
        {
            if (sPromptHirecharchy.Equals("true"))
            {
                PromptHirecharchy = true;
            }
            if (sMultiSet.Equals("true"))
            {
                MultiSet = true;
            }
            if (sIOARequired.Equals("true"))
            {
                IOARequired = true;
            }
            if (sMultiTeacherRequired.Equals("true"))
            {
                MultiTeacherRequired = true;
            }
            if (sIncludeMistrials.Equals("true"))
            {
                IncludeMistrials = true;
            }
        }
        private static int PromptIndex(string[] promptArray, string sPrompt)
        {

            for (int i = 0; i < promptArray.Length; i++)
            {
                if (sPrompt.Trim().Equals(promptArray[i].Trim()))
                {
                    return i;
                }
            }
            return -1;
        }
        public void SetInputData(string sCurrentStep, string sCurrentPrompt, string sTargetPrompt, string sCurrentSet, string sTotalSets, ArrayList Trials)
        {
            try
            {
                CurrentPrompt = sCurrentPrompt;
                TargetPrompt = sTargetPrompt;
                CurrentSet = int.Parse(sCurrentSet);
                TotalSets = int.Parse(sTotalSets);
                if (!TotalTaskMode)
                    CurrentStep = int.Parse(sCurrentStep);

                TrialsData = new TrialAvg[SessionCount];

                Sessions = new Session[SessionCount];
                int iSessionIndex = -1;
                int iTrialIndex = -1;
                foreach (object oData in Trials)
                {
                    string sData = (string)oData;
                    if (sData.Contains("trial"))
                    {
                        if (iSessionIndex != -1)
                        {
                            TrialsData[iSessionIndex].AVGScore = TrialsData[iSessionIndex].AVGScore / StepCount * 100;
                            TrialsData[iSessionIndex].AVGIndependence = TrialsData[iSessionIndex].AVGIndependence / StepCount * 100;
                            TrialsData[iSessionIndex].AVGduration = TrialsData[iSessionIndex].AVGduration / StepCount;
                            for (int j = 0; j < PromptsUsed.Length; j++)
                            {
                                TrialsData[iSessionIndex].PromptAverages[PromptsUsed[j]] = TrialsData[iSessionIndex].PromptAverages[PromptsUsed[j]] / StepCount;
                            }

                        }
                        iSessionIndex++;
                        Sessions[iSessionIndex] = new Session();
                        TrialsData[iSessionIndex] = new TrialAvg();
                        for (int j = 0; j < PromptsUsed.Length; j++)
                        {
                            TrialsData[iSessionIndex].PromptAverages.Add(PromptsUsed[j], 0);
                        }

                        Sessions[iSessionIndex].Steps = new Step[StepCount];
                        iTrialIndex = 0;
                    }
                    else
                    {
                        string[] sTrial = sData.Split(',');
                        Sessions[iSessionIndex].Steps[iTrialIndex] = new Step();
                        Sessions[iSessionIndex].Steps[iTrialIndex].Score = sTrial[1].Trim();
                        if (Model.PromptIndex(PromptsUsed, Sessions[iSessionIndex].Steps[iTrialIndex].Score) >= Model.PromptIndex(PromptsUsed, sCurrentPrompt))
                        {
                            TrialsData[iSessionIndex].AVGScore++;
                            TrialsData[iSessionIndex].Correct++;
                        }
                        if (Sessions[iSessionIndex].Steps[iTrialIndex].Score == "-")
                            TrialsData[iSessionIndex].Incorrect++;
                        Sessions[iSessionIndex].Steps[iTrialIndex].Prompt = sTrial[1].Trim();
                        if (!Sessions[iSessionIndex].Steps[iTrialIndex].Prompt.Contains(":"))
                        {
                            if (PromptsUsed.Length > 0)
                            {
                                if (Sessions[iSessionIndex].Steps[iTrialIndex].Prompt.Equals(PromptsUsed[PromptsUsed.Length - 1]))
                                    TrialsData[iSessionIndex].AVGIndependence++;
                            }
                        }
                        Sessions[iSessionIndex].Steps[iTrialIndex].Duration = ToInt(sTrial[3]);
                        TrialsData[iSessionIndex].AVGduration += Sessions[iSessionIndex].Steps[iTrialIndex].Duration;

                        for (int j = 0; j < PromptsUsed.Length; j++)
                        {
                            if (Sessions[iSessionIndex].Steps[iTrialIndex].Prompt.Equals(PromptsUsed[j]))
                                TrialsData[iSessionIndex].PromptAverages[PromptsUsed[j]] += 1;
                        }

                        if (sTrial[4].Contains("true"))
                        {
                            Sessions[iSessionIndex].Steps[iTrialIndex].Mistrial = true;
                        }
                        else
                        {
                            Sessions[iSessionIndex].Steps[iTrialIndex].Mistrial = false;
                        }
                        iTrialIndex++;
                    }

                }
                TrialsData[iSessionIndex].AVGScore = TrialsData[iSessionIndex].AVGScore / StepCount * 100;
                TrialsData[iSessionIndex].AVGIndependence = TrialsData[iSessionIndex].AVGIndependence / StepCount * 100;
                TrialsData[iSessionIndex].AVGduration = TrialsData[iSessionIndex].AVGduration / StepCount;
                for (int j = 0; j < PromptsUsed.Length; j++)
                {
                    TrialsData[iSessionIndex].PromptAverages[PromptsUsed[j]] = TrialsData[iSessionIndex].PromptAverages[PromptsUsed[j]] / StepCount;
                }

                //Step Averages...

                for (int i = 0; i < Sessions.Length; i++)
                {
                    int istep = 1;
                    float iAccuracyCount = 0;
                    for (int j = 0; j < Sessions[i].Steps.Length; j++)
                    {
                        if (!Sessions[i].Steps[j].Score.Contains(':'))
                        {
                            if (Sessions[i].Steps[j].Score.Equals(Model.PromptIndex(PromptsUsed, sCurrentPrompt)))
                            {
                                iAccuracyCount++;
                            }
                            if (PromptsUsed.Length > 0)
                            {
                                if (Sessions[i].Steps[j].Score.Equals(PromptsUsed[PromptsUsed.Length - 1]))
                                {
                                    Sessions[i].IndependenceCount++;
                                }
                            }
                            Sessions[i].Steps[j].AccuracyPercentage = iAccuracyCount / istep;
                            for (int k = 0; k < PromptsUsed.Length; k++)
                            {
                                if (j > 0)
                                {
                                    Sessions[i].Steps[j].PromptCount = Sessions[i].Steps[j - 1].PromptCount;
                                }
                                if (!Sessions[i].Steps[j].PromptCount.ContainsKey(PromptsUsed[k]))
                                {
                                    Sessions[i].Steps[j].PromptCount.Add(PromptsUsed[k], 0);
                                }
                                // Check prompt <= PromptsUsed[k] index
                                //if (Sessions[i].Steps[j].Prompt.Equals(PromptsUsed[k]))
                                //{
                                //    Sessions[i].Steps[j].PromptCount[PromptsUsed[k]] += 1;
                                //}

                                //if (PromptIndex(PromptsUsed, Sessions[i].Steps[j].Prompt) >= k)
                                //{
                                //    Sessions[i].Steps[j].PromptCount[PromptsUsed[k]] += 1;
                                //}
                                if (j < CurrentStep - 1)
                                {
                                    if (PromptIndex(PromptsUsed, Sessions[i].Steps[j].Prompt) >= PromptIndex(PromptsUsed, TargetPrompt))
                                    {
                                        Sessions[i].Steps[j].PromptCount[PromptsUsed[k]] += 1;
                                    }
                                }
                                else
                                {
                                    if (PromptIndex(PromptsUsed, Sessions[i].Steps[j].Prompt) >= k)
                                    {
                                        Sessions[i].Steps[j].PromptCount[PromptsUsed[k]] += 1;
                                    }
                                }
                                if (!Sessions[i].Steps[j].PromptAverages.ContainsKey(PromptsUsed[k]))
                                {
                                    Sessions[i].Steps[j].PromptAverages.Add(PromptsUsed[k], 0);
                                }

                                Sessions[i].Steps[j].PromptAverages[PromptsUsed[k]] = Sessions[i].Steps[j].PromptCount[PromptsUsed[k]] / istep;
                            }
                            istep++;
                        }
                        Sessions[i].AccuracyCount = iAccuracyCount;
                        Sessions[i].AccuracyPercentageAVG = Sessions[i].AccuracyCount / istep;
                        Sessions[i].IndependentPercentageAVG = Sessions[i].IndependenceCount / istep;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
            }
        }

        public int RequiredSession()
        {
            int max = 0;

            List<int> lTrials = new List<int> { PercentAccuracy.TotalTrial, PercentIndependence.TotalTrial, PercentOpportunity.TotalTrial, 
                MoveBackPercentAccuracy.TotalTrial, MoveBackPercentIndependence.TotalTrial, MoveBackPromptPercentAccuracy.TotalTrial,
                StepMoveBackPercentAccuracy.TotalTrial,StepMoveBackPercentIndependence.TotalTrial,StepPercentAccuracy.TotalTrial,StepPercentIndependence.TotalTrial,
                PromptPercentAccuracy.TotalTrial, PromptPercentIndependence.TotalTrial,CustomPercent.TotalTrial,MoveBackCustom.TotalTrial,
            AvgDurationMoveUp.TotalTrial,AvgDurationMoveDown.TotalTrial,TotalDurationMoveUp.TotalTrial,TotalDurationMoveDown.TotalTrial,
            FrequencyMoveUp.TotalTrial,FrequencyMoveDown.TotalTrial,PromptLearnedStepMoveBack.TotalTrial,PromptLearnedStepMoveUp.TotalTrial,
            PromptExcludeCrntStepMoveUp.TotalTrial,PromptExcludeCrntStepMoveBack.TotalTrial,SetLearnedStepMoveUp.TotalTrial,SetLearnedStepMoveBack.TotalTrial,
            LearnedStepMoveUp.TotalTrial,LearnedStepMoveBack.TotalTrial,PromptTotalCorrectMoveUp.TotalTrial,PromptTotalIncorrectMoveBack.TotalTrial,PromptTotalCorrectMoveBack.TotalTrial,SetExcludeCrntStepMoveUp.TotalTrial,SetExcludeCrntStepMoveBack.TotalTrial,
            StepTotalCorrectMoveUp.TotalTrial,StepTotalIncorrectMoveBack.TotalTrial,StepTotalCorrectMoveBack.TotalTrial,SetTotalCorrectMoveUp.TotalTrial,SetTotalIncorrectMoveBack.TotalTrial,SetTotalCorrectMoveBack.TotalTrial,
            PercentAllIndependence.TotalTrial,MoveBackPercentAllIndependence.TotalTrial,PromptPercentAllIndependence.TotalTrial,MoveBackPromptPercentAllIndependence.TotalTrial,
            StepPercentAllIndependence.TotalTrial,StepMoveBackPercentAllIndependence.TotalTrial,StepFrequencyMoveUp.TotalTrial,PromptFrequencyMoveUp.TotalTrial,StepFrequencyMoveDown.TotalTrial,PromptFrequencyMoveDown.TotalTrial,
            StepAvgDurationMoveUp.TotalTrial,PromptAvgDurationMoveUp.TotalTrial,StepAvgDurationMoveDown.TotalTrial,PromptAvgDurationMoveDown.TotalTrial,StepTotalDurationMoveUp.TotalTrial,PromptTotalDurationMoveUp.TotalTrial,StepTotalDurationMoveDown.TotalTrial,PromptTotalDurationMoveDown.TotalTrial};
            max = lTrials.Max();

            return max;




        }



    }
    public class Result
    {
        public int NextSet = 0;
        public int NextStep = 0;
        public string NextPrompt = "";
        public string CompletionStatus = "";
        public bool PendingIOA = false;
        public bool PendingMultiTeacher = false;
        public bool MovedBackSet = false;
        public bool MovedBackPrompt = false;
        public bool MovedForwardSet = false;
        public bool MovedForwardPrompt = false;
        public bool MovedForwardStep = false;
        public bool MovedBackStep = false;
        public bool MoveBackPromptStep = false;
        public bool MoveForwardPromptStep = false;
        public string[] StepPrompts = null;
        public ArrayList Messages = new ArrayList();
        int mark = 0;
    }
    public static class Model
    {
        static int ToInt(string sValue)
        {
            if (string.IsNullOrEmpty(sValue.Trim()))
            {
                return 0;
            }
            else return int.Parse(sValue);
        }
        public static int PromptIndex(string[] promptArray, string sPrompt)
        {

            for (int i = 0; i < promptArray.Length; i++)
            {
                if (sPrompt.Trim().Equals(promptArray[i].Trim()))
                {
                    return i;
                }
            }
            return -1;
        }
        public static int ConsecutiveCount(float[] sArray, float sCheckFor, bool SuccessCount)
        {
            int maxCount = 0;
            int tempCount = 0;
            for (int i = sArray.Length - 1; i >= 0; i--)
            {
                if (sArray[i] != -9999)
                {
                        if ((SuccessCount && (sArray[i] < sCheckFor / 100)) || (!SuccessCount && (sArray[i] >= sCheckFor / 100)))
                        {
                            if (tempCount > maxCount)
                                maxCount = tempCount;
                            tempCount = 0;
                            break;
                        }
                        else
                            tempCount++;
                }
                //else
                //    tempCount++;
            }
            if (tempCount > maxCount)
                maxCount = tempCount;
            return maxCount;
        }

        public static int ConsecutiveCountfreqd(float[] sArray, float sCheckFor, bool SuccessCount)
        {
            int maxCount = 0;
            int tempCount = 0;
            for (int i = sArray.Length - 1; i >= 0; i--)
            {
                if (sArray[i] != -9999)
                {
                    if (SuccessCount)
                    {
                        if (sArray[i] == 0)
                        {
                            if (tempCount > maxCount)
                                maxCount = tempCount;
                            tempCount = 0;
                            break;
                        }
                        else
                        {
                            tempCount++;
                        }
                    }
                    else
                        if (sArray[i] == 1)
                        {
                            if (tempCount > maxCount)
                                maxCount = tempCount;
                            tempCount = 0;
                            break;
                        }
                        else
                        {
                            tempCount++;
                        }
                }
            }
            if (tempCount > maxCount)
                maxCount = tempCount;
            return maxCount;
        }

        //--- [New Criteria] May 2020 --|ConsecutiveCountTestNew|--(Start)-- //
        public static int ConsecutiveCountTestNew(float[] sArray, float sCheckFor, bool SuccessCount, int TrialVal)
        {
            //int maxCount = 0;
            //int tempCount = 0;

            //float Consavg = 0;
            //for (int i = sArray.Length - 1; i >= 0; i--)
            //{
            //    if (sArray[i] != -9999)
            //    {                    
            //        Consavg = sArray[i];     
            //        if ((SuccessCount && (Consavg < (sCheckFor/100))) || (!SuccessCount && (Consavg > (sCheckFor/100))))
            //        {
            //            if (tempCount > maxCount)
            //                maxCount = tempCount;
            //            tempCount = 0;
            //            break;
            //        }
            //        else
            //            tempCount++;
            //    }
            //    else
            //        tempCount++;
            //}
            float Consavgval = 0;
            for (int i = 0; i < sArray.Length; i++)
            {
                Consavgval += sArray[i];
            }
            Consavgval = Consavgval / TrialVal;
            //if (tempCount > maxCount)
            //    maxCount = tempCount;
            // new average check and return
            Consavgval = Consavgval * 100;
            if (SuccessCount == true)
            {
                if (sArray.Length >= TrialVal && Consavgval >= sCheckFor)
                {
                    return TrialVal;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (sArray.Length >= TrialVal && Consavgval < sCheckFor)
                {
                    return TrialVal;
                }
                else
                {
                    return 0;
                }

            }

        }
        //--- [New Criteria] May 2020 --|ConsecutiveCountTestNew|--(End)-- //

        // consecutive avg duration and frequency Start//
        public static int ConsecutiveCountTestNewAvgDur(float[] sArray, float sCheckFor, bool SuccessCount, int TrialVal,int fredurStat)
        {
           
            float Consavgval = 0;
            for (int i = 0; i < sArray.Length; i++)
            {
                Consavgval += sArray[i];
            }
            Consavgval = Consavgval / TrialVal;
            if (fredurStat == 1)
            {

                if (SuccessCount == true)
                {
                    if (sArray.Length >= TrialVal && Consavgval < sCheckFor)
                    {
                        return TrialVal;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    if (sArray.Length >= TrialVal && Consavgval > sCheckFor)
                    {
                        return TrialVal;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            else
            {
                if (SuccessCount == true)
                {
                    if (sArray.Length >= TrialVal && Consavgval > sCheckFor)
                    {
                        return TrialVal;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    if (sArray.Length >= TrialVal && Consavgval < sCheckFor)
                    {
                        return TrialVal;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            

        }
        // consecutive avg duration and frequency End// 



        public static int ConsecutiveCountCorrect(float[] sArray, float sCheckFor, bool SuccessCount)
        {
            int maxCount = 0;
            int tempCount = 0;
            for (int i = sArray.Length - 1; i >= 0; i--)
            {
                if ((SuccessCount && (sArray[i] < sCheckFor) || sArray[i] == 0) || (!SuccessCount && (sArray[i] >= sCheckFor) || sArray[i] == 0))
                {
                    if (tempCount > maxCount)
                        maxCount = tempCount;
                    tempCount = 0;
                    break;
                }
                else
                    tempCount++;
            }
            if (tempCount > maxCount)
                maxCount = tempCount;
            return maxCount;
        }

        //--- [New Criteria] May 2020 --|ConsecutiveCountCorrectTestNew|--(Start)-- //
        public static int ConsecutiveCountCorrectTestNew(float[] sArray, float sCheckFor, bool SuccessCount)
        {
            int maxCount = 0;
            int tempCount = 0;
            for (int i = sArray.Length - 1; i >= 0; i--)
            {
                if ((SuccessCount && (sArray[i] < sCheckFor) || sArray[i] == 0) || (!SuccessCount && (sArray[i] >= sCheckFor) || sArray[i] == 0))
                {
                    if (tempCount > maxCount)
                        maxCount = tempCount;
                    tempCount = 0;
                    break;
                }
                else
                    tempCount++;
            }
            if (tempCount > maxCount)
                maxCount = tempCount;
            return maxCount;
        }
        //--- [New Criteria] May 2020 --|ConsecutiveCountCorrectTestNew|--(End)-- //

        // consecutive avg total correct start//
        public static int ConsecutiveCountTestNewCorrect(float[] sArray, float sCheckFor, bool SuccessCount, int TrialVal)
        {
           
            float Consavgval = 0;
            for (int i = 0; i < sArray.Length; i++)
            {
                Consavgval += sArray[i];
            }
            Consavgval = Consavgval / TrialVal;
            if (SuccessCount == true)
            {
                if (sArray.Length >= TrialVal && Consavgval >= sCheckFor)
                {
                    return TrialVal;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (sArray.Length >= TrialVal && Consavgval < sCheckFor)
                {
                    return TrialVal;
                }
                else
                {
                    return 0;
                }

            }

        }
        // consecutive avg total correct End//
        public static int SuccessORFailureCount(float[] sArray, float sCheckFor, int iTrialCount, bool SuccessCount)
        {
            int tempCount = 0;
            int iStart = sArray.Length - iTrialCount;
            if (iStart < 0)
                iStart = 0;

            for (int i = 0; i < sArray.Length; i++)
            {
                if (sArray[i] != -9999)
                {
                    if ((SuccessCount && (sArray[i] >= sCheckFor / 100)) || (!SuccessCount && (sArray[i] < sCheckFor / 100)))
                    {
                        tempCount++;
                    }
                }
                else
                    tempCount++;
            }
            return tempCount;
        }

        public static int SuccessORFailureCountFrequencyDuration(float[] sArray, float sCheckFor, int iTrialCount, bool SuccessCount)
        {
            int tempCount = 0;
            int iStart = sArray.Length - iTrialCount;
            if (iStart < 0)
                iStart = 0;

            for (int i = 0; i < sArray.Length; i++)
            {
                if (sArray[i] != -9999)
                {
                    if ((SuccessCount && (sArray[i] == 1)) || (!SuccessCount && (sArray[i] == 0)))
                    {
                        tempCount++;
                    }
                }
                else
                    tempCount++;
            }
            return tempCount;
        }

        //--- [New Criteria] May 2020 --|SuccessORFailureCountTestNew|--(Start)-- //
        public static int SuccessORFailureCountTestNew(float[] sArray, float sCheckFor, int iTrialCount, bool SuccessCount)
        {
            int tempCount = 0;
            int iStart = sArray.Length - iTrialCount;
            if (iStart < 0)
                iStart = 0;

            float Consavg = 0;
            for (int i = 0; i < sArray.Length; i++)
            {
                Consavg += sArray[i];
                Consavg = Consavg / iTrialCount;
                Consavg = Consavg * 100;
            }

            for (int i = 0; i < sArray.Length; i++)
            {
                if (sArray[i] != -9999)
                {
                    if ((SuccessCount && (sArray[i] >= sCheckFor / Consavg)) || (!SuccessCount && (sArray[i] <= sCheckFor / Consavg)))
                    {
                        tempCount++;
                    }
                }
                else
                    tempCount++;
            }
            return tempCount;
        }
        //--- [New Criteria] May 2020 --|SuccessORFailureCountTestNew|--(End)-- //

        public static int SuccessORFailureCountCorrect(float[] sArray, float sCheckFor, int iTrialCount, bool SuccessCount)
        {
            int tempCount = 0;
            int iStart = sArray.Length - iTrialCount;
            if (iStart < 0)
                iStart = 0;

            for (int i = iStart; i < sArray.Length; i++)
            {
                if ((SuccessCount && (sArray[i] >= sCheckFor)) || (!SuccessCount && (((sArray[i] < sCheckFor)))))
                {
                    tempCount++;
                }

            }
            return tempCount;
        }

        //--- [New Criteria] May 2020 --|SuccessORFailureCountCorrectTestNew|--(Start)-- //
        public static int SuccessORFailureCountCorrectTestNew(float[] sArray, float sCheckFor, int iTrialCount, bool SuccessCount)
        {
            int tempCount = 0;
            int iStart = sArray.Length - iTrialCount;
            if (iStart < 0)
                iStart = 0;

            for (int i = iStart; i < sArray.Length; i++)
            {
                if ((SuccessCount && (sArray[i] >= sCheckFor)) || (!SuccessCount && (((sArray[i] <= sCheckFor) && sArray[i] > 0) || ((sArray[i] == sCheckFor) && sCheckFor == 0))))
                {
                    tempCount++;
                }

            }
            return tempCount;
        }
        //--- [New Criteria] May 2020 --|SuccessORFailureCountCorrectTestNew|--(End)-- //

        public static Result GetNextStepAndPrompt(InputData data, bool bPromptColumn)
        {

            int iStartIndex = PromptIndex(data.PromptsUsed, data.CurrentPrompt);
            int iStartStep = data.CurrentStep - 1;
            Result res = new Result();
            int curr = data.next;
            bool bsucs = true;
            int x;
            double avgscore=0;
            double totscore = 0;
            float[] avgCorrect = new float[data.Sessions.Length];
            float[] avgIncorrect = new float[data.Sessions.Length];
            bool bSuccessPrompt = false;
            bool bSuccessStep = false;
            for (int i = 0; i < data.TrialsData.Length; i++)
            {
                avgCorrect[i] = data.TrialsData[i].Correct;
                avgIncorrect[i] = data.TrialsData[i].Incorrect;
            }
            for (int j = iStartStep; j < data.Sessions[0].Steps.Length; j++)
            {
                bool bSuccess = false;
                float[] avgInd = new float[data.Sessions.Length];
                float[] avgIndAll = new float[data.Sessions.Length];
                float[] avgCurrent = new float[data.Sessions.Length];

                float[] avgLearned = new float[data.Sessions.Length];
                float[] avgExcludeCrnt = new float[data.Sessions.Length];
                float CrctRespCountat = 0;

                float[] avgPrmFre = new float[data.Sessions.Length];
                float[] avgStpFre = new float[data.Sessions.Length];
                float[] avgPrmAvgDur = new float[data.Sessions.Length];
                float[] avgStpAvgDur = new float[data.Sessions.Length];
                float[] avgPrmTotDur = new float[data.Sessions.Length];
                float[] avgStpTotDur = new float[data.Sessions.Length];

                float[] avgPrmFreNew = new float[data.Sessions.Length];
                float[] avgStpFreNew = new float[data.Sessions.Length];
                float[] avgPrmAvgDurNew = new float[data.Sessions.Length];
                float[] avgPrmTotDurNew = new float[data.Sessions.Length];
                float[] avgStpAvgDurNew = new float[data.Sessions.Length];
                float[] avgStpTotDurNew = new float[data.Sessions.Length];

                if (data.PromptPercentAllIndependence.TotalTrial > 0)
                {
                    x = PromptIndex(data.PromptsUsed, data.CurrentPrompt) + 1;
                }
                else
                {
                    x = data.PromptsUsed.Length;
                }

                for (int i = iStartIndex; i < x; i++)
                {
                    bSuccess = true;
                    bSuccessPrompt = true;
                    float[] avgAcc = new float[data.Sessions.Length];

                    for (int k = 0; k < data.Sessions.Length; k++)
                    {
                        if (!data.Sessions[k].Steps[j].Score.Contains(":"))
                        {
                            if ((data.Sessions[k].Steps[j].Score != null) && (data.Sessions[k].Steps[j].Score.ToString() != ""))
                            {
                                if (data.PromptsUsed.Length > 0)
                                {

                                    if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                    {
                                        avgAcc[k] = 1;
                                    }

                                    else
                                    {
                                        avgAcc[k] = 0;
                                    }
                                }
                                if (data.PromptsUsed.Length > 0)
                                {
                                    if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) >= data.PromptsUsed.Length - 1)
                                    {
                                        avgInd[k] = 1;
                                    }
                                    else
                                    {
                                        avgInd[k] = 0;
                                    }
                                }

                                //%independent of all steps
                                //if (data.PercentAllIndependence.TotalTrial>0|| data.StepPercentAllIndependence.TotalTrial>0 || data.PromptPercentAllIndependence.TotalTrial>0)
                                //{
                                //    if (data.Sessions[k].Steps[j].Score == "+")
                                //    {
                                //        if (data.sCurrentLessonPrompt == "118")
                                //        {
                                //            avgIndAll[k] = 1;
                                //        }
                                //        else
                                //        {
                                //            avgIndAll[k] = 0;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        if (data.Sessions[k].Steps[j].Score == "118")
                                //        {
                                //            avgIndAll[k] = 1;
                                //        }
                                //        else
                                //        {
                                //            avgIndAll[k] = 0;
                                //        }
                                //    }
                                //}

                                if (data.PromptsUsed.Length > 0)
                                {
                                    if (data.PromptsUsed[i] == data.TargetPrompt)
                                    {
                                        avgCurrent[k] = data.Sessions[k].Steps[j].PromptAverages[data.TargetPrompt];
                                    }
                                }

                                //train
                                if (data.PromptLearnedStepMoveUp.TotalTrial > 0 || data.LearnedStepMoveUp.TotalTrial > 0 || data.SetLearnedStepMoveUp.TotalTrial > 0)
                                {
                                    if (data.next == data.StepCount)
                                    {
                                        data.next = data.next - 1;
                                    }
                                    if (data.next == 0)
                                    {
                                        data.next = 1;
                                    }
                                    if (data.PromptsUsed.Length > 0)
                                    {
                                        if (data.Sessions[k].Steps[data.next - 1].Score == data.CorrectResp)
                                        {
                                            CrctRespCountat++;
                                        }
                                    }
                                    avgLearned[k] = CrctRespCountat;
                                    CrctRespCountat = 0;
                                }

                                if (data.PromptFrequencyMoveUp.TotalTrial > 0)
                                {
                                    int GetStat = data.FreDurMoveStat;
                                    string getPromptScore = "";                                    

                                    if (GetStat == 1)
                                    {
                                        if (Convert.ToInt32(data.Sessions[k].Steps[j].Score) < data.PromptFrequencyMoveUp.BarCondition)
                                        {
                                            getPromptScore = "+";
                            }
                            else
                            {
                                            getPromptScore = "-";
                                        }
                                    }
                                    else if (GetStat == 0)
                                    {
                                        if (Convert.ToInt32(data.Sessions[k].Steps[j].Score) > data.PromptFrequencyMoveUp.BarCondition)
                                        {
                                            getPromptScore = "+";
                                        }
                                        else
                                        {
                                            getPromptScore = "-";
                                        }
                                    }

                                if (data.PromptsUsed.Length > 0)
                                {
                                    float frscore=Convert.ToInt32(data.Sessions[k].Steps[j].Score);
                                    avgPrmFreNew[k] = frscore;

                                        if (PromptIndex(data.PromptsUsed, getPromptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                        {
                                            avgPrmFre[k] = 1;
                                        }

                                        else
                                        {
                                            avgPrmFre[k] = 0;
                                        }
                                    }
                                }

                                //if (data.PromptAvgDurationMoveUp.TotalTrial > 0)
                                //{
                                //    int GetStat = data.FreDurMoveStat;
                                //    string getPromptScore = "";
                                //    double getTime = TimeSpan.Parse(data.Sessions[k].Steps[data.CurrentStep - 1].Score).TotalSeconds;

                                //    if (GetStat == 1)
                                //    {
                                //        if (Convert.ToInt32(getTime) < data.PromptAvgDurationMoveUp.BarCondition)
                                //        {
                                //            getPromptScore = "+";
                                //        }
                                //        else
                                //        {
                                //            getPromptScore = "-";
                                //        }
                                //    }
                                //    else if (GetStat == 0)
                                //    {
                                //        if (Convert.ToInt32(getTime) > data.PromptAvgDurationMoveUp.BarCondition)
                                //        {
                                //            getPromptScore = "+";
                                //        }
                                //        else
                                //        {
                                //            getPromptScore = "-";
                                //        }
                                //    }

                                //    if (data.PromptsUsed.Length > 0)
                                //    {
                                //        if (PromptIndex(data.PromptsUsed, getPromptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                //        {
                                //            avgPrmAvgDur[k] = 1;
                                //        }

                                //        else
                                //        {
                                //            avgPrmAvgDur[k] = 0;
                                //        }
                                //    }
                                //}

                                //if (data.PromptTotalDurationMoveUp.TotalTrial > 0)
                                //{
                                //    int GetStat = data.FreDurMoveStat;
                                //    string getPromptScore = "";
                                //    double getTime = TimeSpan.Parse(data.Sessions[k].Steps[data.CurrentStep - 1].Score).TotalSeconds;

                                //    if (GetStat == 1)
                                //    {
                                //        if (Convert.ToInt32(getTime) < data.PromptTotalDurationMoveUp.BarCondition)
                                //        {
                                //            getPromptScore = "+";
                                //        }
                                //        else
                                //        {
                                //            getPromptScore = "-";
                                //        }
                                //    }
                                //    else if (GetStat == 0)
                                //    {
                                //        if (Convert.ToInt32(getTime) > data.PromptTotalDurationMoveUp.BarCondition)
                                //        {
                                //            getPromptScore = "+";
                                //        }
                                //        else
                                //        {
                                //            getPromptScore = "-";
                                //        }
                                //    }

                                //    if (data.PromptsUsed.Length > 0)
                                //    {
                                //        if (PromptIndex(data.PromptsUsed, getPromptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                //        {
                                //            avgPrmTotDur[k] = 1;
                                //        }

                                //        else
                                //        {
                                //            avgPrmTotDur[k] = 0;
                                //        }
                                //    }
                                //}

                            }
                            else
                            {
                                if (data.PromptsUsed.Length > 0)
                                {
                                    avgAcc[k] = -9999;
                                    avgInd[k] = -9999;
                                    avgIndAll[k] = -9999;
                                }
                            }
                            if (data.PromptExcludeCrntStepMoveUp.TotalTrial > 0 || data.ExcludeCrntStepMoveUp.TotalTrial > 0 || data.SetExcludeCrntStepMoveUp.TotalTrial > 0)
                            {
                                if (data.next == data.StepCount)
                                {
                                    data.next = data.next - 1;
                                }
                                if (data.next == 0)
                                {
                                    data.next = 1;
                                }
                                if ((data.Sessions[k].Steps[j].Score != null) && (data.Sessions[k].Steps[j].Score.ToString() != ""))
                                {
                                    if (data.PromptsUsed.Length > 0)
                                    {
                                        if (avgCorrect[k] + avgIncorrect[k] == 1 || avgCorrect[k] + avgIncorrect[k] == 0)
                                        {
                                            if ((data.Sessions[k].Steps[data.next - 1].Score != null) && (data.Sessions[k].Steps[data.next - 1].Score.ToString() != ""))
                                            {
                                                avgExcludeCrnt[k] = 0;
                                            }
                                            else
                                            {

                                                if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                                {
                                                    avgExcludeCrnt[k] = 1;
                                                }

                                                else
                                                {
                                                    avgExcludeCrnt[k] = 0;
                                                }

                                            }

                                        }
                                        else if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                        {
                                            avgExcludeCrnt[k] = 1;
                                        }

                                        else
                                        {
                                            if (j == data.next - 1)
                                            {
                                                avgExcludeCrnt[k] = 1;
                                            }
                                            else
                                            {
                                                avgExcludeCrnt[k] = 0;
                                            }
                                        }


                                    }
                                }
                                else
                                {
                                    if (data.PromptsUsed.Length > 0)
                                    {

                                        avgExcludeCrnt[k] = 1;
                                    }


                                }
                            }
                        }
                        else if (data.Sessions[k].Steps[j].Score.Contains(":") && (data.PromptAvgDurationMoveUp.TotalTrial > 0 || data.PromptTotalDurationMoveUp.TotalTrial > 0))
                        {
                            if (data.PromptAvgDurationMoveUp.TotalTrial > 0)
                            {
                                int GetStat = data.FreDurMoveStat;
                                string getPromptScore = "";
                                double getTime = TimeSpan.Parse(data.Sessions[k].Steps[data.CurrentStep - 1].Score).TotalSeconds;
                             

                                if (GetStat == 1)
                                {
                                    if (Convert.ToInt32(getTime) < data.PromptAvgDurationMoveUp.BarCondition)
                                    {
                                        getPromptScore = "+";
                                    }
                                    else
                                    {
                                        getPromptScore = "-";
                                    }
                                }
                                else if (GetStat == 0)
                                {
                                    if (Convert.ToInt32(getTime) > data.PromptAvgDurationMoveUp.BarCondition)
                                    {
                                        getPromptScore = "+";
                                    }
                                    else
                                    {
                                        getPromptScore = "-";
                                    }
                                }

                                if (data.PromptsUsed.Length > 0)
                                {
                                    float avgdur = (float)getTime;
                                    avgPrmAvgDurNew[k] = avgdur;
                                    if (PromptIndex(data.PromptsUsed, getPromptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                    {
                                        avgPrmAvgDur[k] = 1;
                                    }

                                    else
                                    {
                                        avgPrmAvgDur[k] = 0;
                        }
                                    
                                   
                                }
                            }


                            

                            if (data.PromptTotalDurationMoveUp.TotalTrial > 0)
                            {
                                int GetStat = data.FreDurMoveStat;
                                string getPromptScore = "";
                                double getTime = TimeSpan.Parse(data.Sessions[k].Steps[data.CurrentStep - 1].Score).TotalSeconds;
                                
                                

                                if (GetStat == 1)
                                {
                                    if (Convert.ToInt32(getTime) < data.PromptTotalDurationMoveUp.BarCondition)
                                    {
                                        getPromptScore = "+";
                                    }
                        else
                        {
                                        getPromptScore = "-";
                                    }
                                }
                                else if (GetStat == 0)
                                {
                                    if (Convert.ToInt32(getTime) > data.PromptTotalDurationMoveUp.BarCondition)
                                    {
                                        getPromptScore = "+";
                                    }
                                    else
                                    {
                                        getPromptScore = "-";
                                    }
                                }

                                if (data.PromptsUsed.Length > 0)
                                {
                                    float totdur = (float)getTime;
                                    avgPrmTotDurNew[k] = totdur;
                                    if (PromptIndex(data.PromptsUsed, getPromptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                    {
                                        avgPrmTotDur[k] = 1;
                                    }

                                    else
                                    {
                                        avgPrmTotDur[k] = 0;
                                    }
                                   
                                }
                            }
                            
                        }
                        else
                        {
                            bSuccess = false;
                            bSuccessPrompt = false;
                        }
                       

                    }

                    
                    if (data.PromptFrequencyMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|PromptFrequencyMoveUp|--(Start)-- //
                        if (data.PromptFrequencyMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgPrmFreNew, data.PromptFrequencyMoveUp.BarCondition, true, data.PromptFrequencyMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.PromptFrequencyMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|PromptFrequencyMoveUp|--(End)-- //
                        else if (data.PromptFrequencyMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgPrmFre, data.PromptFrequencyMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.PromptFrequencyMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgPrmFre, data.PromptFrequencyMoveUp.BarCondition, data.PromptFrequencyMoveUp.TotalTrial, true);
                            if (AccCount < data.PromptFrequencyMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }

                    if (data.PromptAvgDurationMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|PromptAvgDurationMoveUp|--(Start)-- //
                        if (data.PromptAvgDurationMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgPrmAvgDurNew, data.PromptAvgDurationMoveUp.BarCondition, true, data.PromptAvgDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.PromptAvgDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|PromptAvgDurationMoveUp|--(End)-- //
                        else if (data.PromptAvgDurationMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgPrmAvgDur, data.PromptAvgDurationMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.PromptAvgDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgPrmAvgDur, data.PromptAvgDurationMoveUp.BarCondition, data.PromptAvgDurationMoveUp.TotalTrial, true);
                            if (AccCount < data.PromptAvgDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }

                    if (data.PromptTotalDurationMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|PromptTotalDurationMoveUp|--(Start)-- //
                        if (data.PromptTotalDurationMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgPrmTotDurNew, data.PromptTotalDurationMoveUp.BarCondition, true, data.PromptTotalDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.PromptTotalDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|PromptTotalDurationMoveUp|--(End)-- //
                        else if (data.PromptTotalDurationMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgPrmTotDur, data.PromptTotalDurationMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.PromptTotalDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgPrmTotDur, data.PromptTotalDurationMoveUp.BarCondition, data.PromptTotalDurationMoveUp.TotalTrial, true);
                            if (AccCount < data.PromptTotalDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }

                    if (data.PromptPercentAccuracy.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(Start)-- //
                        if (data.PromptPercentAccuracy.ConsecutiveAverage) 
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgAcc, data.PromptPercentAccuracy.BarCondition, true, data.PromptPercentAccuracy.TotalTrial);
                            if (AccConsecutiveCount < data.PromptPercentAccuracy.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(End)-- //
                        else if (data.PromptPercentAccuracy.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgAcc, data.PromptPercentAccuracy.BarCondition, true);
                            if (AccConsecutiveCount < data.PromptPercentAccuracy.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgAcc, data.PromptPercentAccuracy.BarCondition, data.PromptPercentAccuracy.TotalTrial, true);
                            if (AccCount < data.PromptPercentAccuracy.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }
                    if (data.PromptPercentIndependence.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|PromptPercentIndependence|--(Start)-- //
                        if (data.PromptPercentIndependence.ConsecutiveAverage)
                        {
                            int IndConsecutiveCount = ConsecutiveCountTestNew(avgInd, data.PromptPercentIndependence.BarCondition, true, data.PromptPercentIndependence.TotalTrial);
                            if (IndConsecutiveCount < data.PromptPercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|PromptPercentIndependence|--(End)-- //
                        else if (data.PromptPercentIndependence.ConsecutiveSuccess)
                        {
                            int IndConsecutiveCount = ConsecutiveCount(avgInd, data.PromptPercentIndependence.BarCondition, true);
                            if (IndConsecutiveCount < data.PromptPercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int IndCount = SuccessORFailureCount(avgInd, data.PromptPercentIndependence.BarCondition, data.PromptPercentIndependence.TotalTrial, true);
                            if (IndCount < data.PromptPercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }

                    }
                    //%independent of all steps
                    //if (data.PromptPercentAllIndependence.TotalTrial > 0)
                    //{
                    //    if (data.PromptPercentAllIndependence.ConsecutiveSuccess)
                    //    {
                    //        int IndConsecutiveCount = ConsecutiveCount(avgIndAll, data.PromptPercentAllIndependence.BarCondition, true);
                    //        if (IndConsecutiveCount < data.PromptPercentAllIndependence.SuccessNeeded)
                    //        {
                    //            bSuccess = false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        int IndCount = SuccessORFailureCount(avgIndAll, data.PromptPercentAllIndependence.BarCondition, data.PromptPercentAllIndependence.TotalTrial, true);
                    //        if (IndCount < data.PromptPercentAllIndependence.SuccessNeeded)
                    //        {
                    //            bSuccess = false;
                    //        }
                    //    }

                    //}
                    if (data.PromptTotalCorrectMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|PromptTotalCorrectMoveUp|--(Start)-- //
                        if (data.PromptTotalCorrectMoveUp.ConsecutiveAverage)
                        {
                            int TCConsecutiveCount = ConsecutiveCountTestNewCorrect(avgCorrect, data.PromptTotalCorrectMoveUp.BarCondition, true, data.PromptTotalCorrectMoveUp.TotalTrial);
                            if (TCConsecutiveCount < data.PromptTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|PromptTotalCorrectMoveUp|--(End)-- //
                        else if (data.PromptTotalCorrectMoveUp.ConsecutiveSuccess)
                        {
                            int TCConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, data.PromptTotalCorrectMoveUp.BarCondition, true);
                            if (TCConsecutiveCount < data.PromptTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int TCCount = SuccessORFailureCountCorrect(avgCorrect, data.PromptTotalCorrectMoveUp.BarCondition, data.PromptTotalCorrectMoveUp.TotalTrial, true);
                            if (TCCount < data.PromptTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }
                    if (data.PromptLearnedStepMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|PromptLearnedStepMoveUp|--(Start)-- //
                        if (data.PromptLearnedStepMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgLearned, data.PromptLearnedStepMoveUp.BarCondition, true, data.PromptLearnedStepMoveUp.TotalTrial);
                            if (AccConsecutiveCount < data.PromptLearnedStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|PromptLearnedStepMoveUp|--(End)-- //
                        else if (data.PromptLearnedStepMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgLearned, data.PromptLearnedStepMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.PromptLearnedStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                                //bsucs = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgLearned, data.PromptLearnedStepMoveUp.BarCondition, data.PromptLearnedStepMoveUp.TotalTrial, true);
                            if (AccCount < data.PromptLearnedStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                                //bsucs = false;
                            }
                        }
                    }

                    if (data.PromptExcludeCrntStepMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|PromptExcludeCrntStepMoveUp|--(Start)-- //
                        if (data.PromptExcludeCrntStepMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgExcludeCrnt, data.PromptExcludeCrntStepMoveUp.BarCondition, true, data.PromptExcludeCrntStepMoveUp.TotalTrial);
                            if (AccConsecutiveCount < data.PromptExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|PromptExcludeCrntStepMoveUp|--(End)-- //
                        else if (data.PromptExcludeCrntStepMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgExcludeCrnt, data.PromptExcludeCrntStepMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.PromptExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                                bsucs = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgExcludeCrnt, data.PromptExcludeCrntStepMoveUp.BarCondition, data.PromptExcludeCrntStepMoveUp.TotalTrial, true);
                            if (AccCount < data.PromptExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                                bsucs = false;
                            }
                        }
                    }


                    if ((data.PromptPercentIndependence.TotalTrial == 0 && data.PromptPercentAccuracy.TotalTrial == 0 && data.PromptTotalCorrectMoveUp.TotalTrial == 0 && data.PromptExcludeCrntStepMoveUp.TotalTrial == 0 && data.PromptLearnedStepMoveUp.TotalTrial == 0 && data.PromptFrequencyMoveUp.TotalTrial == 0 && data.PromptAvgDurationMoveUp.TotalTrial == 0 && data.PromptTotalDurationMoveUp.TotalTrial == 0) && (data.promptUp == 0))
                    {
                        bSuccess = false;
                        bSuccessPrompt = false;
                    }
                    if (bSuccess)
                    {

                        if (data.PromptsUsed.Length > 0)
                        {
                            res.NextPrompt = data.PromptsUsed[i];
                        }
                        if (!bPromptColumn)
                        {
                            if (data.PromptHirecharchy)
                            {
                                if (data.NoPromptsUsed.Length > 0)
                                {
                                    if (res.NextPrompt == "+")
                                        res.MovedForwardPrompt = true;
                                }
                            }
                        }
                        res.NextStep = j + 1;
                        iStartIndex = 0;

                    }

                }
                if (bPromptColumn)
                {
                    if (PromptIndex(data.PromptsUsed, data.TargetPrompt) > PromptIndex(data.PromptsUsed, data.CurrentPrompt))
                    {
                        if (PromptIndex(data.PromptsUsed, data.TargetPrompt) == PromptIndex(data.PromptsUsed, res.NextPrompt))
                        {
                            res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, res.NextPrompt)];
                        }
                        else
                            res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, res.NextPrompt) + 1];
                        return res;
                    }
                    res.NextStep = j + 1;
                    iStartIndex = 0;
                }
                else
                {
                    if ((data.NoPromptsUsed.Length > 0) && (data.promptUp == 0))
                    {
                        if (PromptIndex(data.NoPromptsUsed, data.NoPromptsUsed[data.NoPromptsUsed.Length - 1]) > PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt))
                        {
                            if (PromptIndex(data.NoPromptsUsed, data.NoPromptsUsed[data.NoPromptsUsed.Length - 1]) == PromptIndex(data.NoPromptsUsed, res.NextPrompt))
                            {
                                res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, res.NextPrompt)];
                            }
                            else
                                res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, res.NextPrompt) + 1];
                            return res;
                        }
                    }
                    else
                    {
                        if (PromptIndex(data.PromptsUsed, data.TargetPrompt) > PromptIndex(data.PromptsUsed, data.CurrentPrompt))
                        {
                            if (PromptIndex(data.PromptsUsed, data.TargetPrompt) == PromptIndex(data.PromptsUsed, res.NextPrompt))
                            {
                                res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, res.NextPrompt)];
                            }
                            else
                                res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, res.NextPrompt) + 1];
                            return res;
                        }
                    }
                }
                //Check for Step Move , if failed, then set prompt as target prompt and return
                //if success then move step by 1 and make prompt as 0 index and continue in loop

                //Put a LOOP for prompt, as it can be TargetPrompt or Above...
                bool bAtleastOneSuccess = false;
                for (int i = PromptIndex(data.PromptsUsed, data.TargetPrompt); i < data.PromptsUsed.Length; i++)
                {



                    bSuccess = true;
                    bSuccessStep = true;
                    for (int k = 0; k < data.Sessions.Length; k++)
                    {
                        if (!data.Sessions[k].Steps[j].Score.Contains(':') && data.PromptsUsed.Length > 0)
                        {
                            if ((data.Sessions[k].Steps[j].Score != null) && (data.Sessions[k].Steps[j].Score.ToString() != ""))
                            {
                                if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                {
                                    avgCurrent[k] = 1;
                                }
                                else
                                {
                                    avgCurrent[k] = 0;
                                }
                                if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) == (data.PromptsUsed.Length - 1))
                                {
                                    avgInd[k] = 1;
                                }
                                else
                                {
                                    avgInd[k] = 0;
                                }
                                //independendent of all steps
                                //if (data.PercentAllIndependence.TotalTrial > 0 || data.StepPercentAllIndependence.TotalTrial > 0 || data.PromptPercentAllIndependence.TotalTrial > 0)
                                //{
                                //    if (data.Sessions[k].Steps[j].Score == "+")
                                //    {
                                //        if (data.sCurrentLessonPrompt == "118")
                                //        {
                                //            avgIndAll[k] = 1;
                                //        }
                                //        else
                                //        {
                                //            avgIndAll[k] = 0;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        if (data.Sessions[k].Steps[j].Score == "118")
                                //        {
                                //            avgIndAll[k] = 1;
                                //        }
                                //        else
                                //        {
                                //            avgIndAll[k] = 0;
                                //        }
                                //    }
                                //}
                                //train
                                if (data.PromptLearnedStepMoveUp.TotalTrial > 0 || data.LearnedStepMoveUp.TotalTrial > 0 || data.SetLearnedStepMoveUp.TotalTrial > 0)
                                {
                                    if (data.next == data.StepCount)
                                    {
                                        data.next = data.next - 1;
                                    }
                                    if (data.next == 0)
                                    {
                                        data.next = 1;
                                    }
                                    if (data.PromptLearnedStepMoveUp.TotalTrial > 0)
                                    {
                                        if (data.PromptsUsed.Length > 0)
                                        {
                                            if (data.Sessions[k].Steps[data.next - 1].Score == data.CorrectResp)
                                            {
                                                CrctRespCountat++;
                                            }
                                        }
                                        avgLearned[k] = CrctRespCountat;
                                        CrctRespCountat = 0;
                                    }
                                }

                                if (data.StepFrequencyMoveUp.TotalTrial > 0)
                                {
                                    int GetStat = data.FreDurMoveStat;
                                    string getStepScore = "";

                                    if (GetStat == 1)
                                    {
                                        if (Convert.ToInt32(data.Sessions[k].Steps[j].Score) < data.StepFrequencyMoveUp.BarCondition)
                                        {
                                            getStepScore = "+";
                            }
                            else
                            {
                                            getStepScore = "-";
                                        }
                                    }
                                    else if (GetStat == 0)
                                    {
                                        if (Convert.ToInt32(data.Sessions[k].Steps[j].Score) > data.StepFrequencyMoveUp.BarCondition)
                                        {
                                            getStepScore = "+";
                                        }
                                        else
                                        {
                                            getStepScore = "-";
                                        }
                                    }

                                    if (data.PromptsUsed.Length > 0)
                                    {
                                        float frscore = Convert.ToInt32(data.Sessions[k].Steps[j].Score);
                                        avgStpFreNew[k] = frscore;
                                        if (PromptIndex(data.PromptsUsed, getStepScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                        {
                                            avgStpFre[k] = 1;
                                        }

                                        else
                                        {
                                            avgStpFre[k] = 0;
                                        }
                                    }
                                }

                                //if (data.StepAvgDurationMoveUp.TotalTrial > 0)
                                //{
                                //    int GetStat = data.FreDurMoveStat;
                                //    string getStepScore = "";
                                //    double getTime = TimeSpan.Parse(data.Sessions[k].Steps[data.CurrentStep - 1].Score).TotalSeconds;

                                //    if (GetStat == 1)
                                //    {
                                //        if (Convert.ToInt32(getTime) < data.StepAvgDurationMoveUp.BarCondition)
                                //        {
                                //            getStepScore = "+";
                                //        }
                                //        else
                                //        {
                                //            getStepScore = "-";
                                //        }
                                //    }
                                //    else if (GetStat == 0)
                                //    {
                                //        if (Convert.ToInt32(getTime) > data.StepAvgDurationMoveUp.BarCondition)
                                //        {
                                //            getStepScore = "+";
                                //        }
                                //        else
                                //        {
                                //            getStepScore = "-";
                                //        }
                                //    }
                                    
                                //    if (data.PromptsUsed.Length > 0)
                                //    {
                                //        if (PromptIndex(data.PromptsUsed, getStepScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                //        {
                                //            avgStpFre[k] = 1;
                                //        }
                                //        else
                                //        {
                                //            avgStpFre[k] = 0;
                                //        }
                                //    }
                                //}

                                //if (data.StepTotalDurationMoveUp.TotalTrial > 0)
                                //{
                                //    int GetStat = data.FreDurMoveStat;
                                //    string getStepScore = "";
                                //    double getTime = TimeSpan.Parse(data.Sessions[k].Steps[data.CurrentStep - 1].Score).TotalSeconds;

                                //    if (GetStat == 1)
                                //    {
                                //        if (Convert.ToInt32(data.Sessions[k].Steps[j].Score) < data.StepTotalDurationMoveUp.BarCondition)
                                //        {
                                //            getStepScore = "+";
                                //        }
                                //        else
                                //        {
                                //            getStepScore = "-";
                                //        }
                                //    }
                                //    else if (GetStat == 0)
                                //    {
                                //        if (Convert.ToInt32(data.Sessions[k].Steps[j].Score) > data.StepTotalDurationMoveUp.BarCondition)
                                //        {
                                //            getStepScore = "+";
                                //        }
                                //        else
                                //        {
                                //            getStepScore = "-";
                                //        }
                                //    }                                    

                                //    if (data.PromptsUsed.Length > 0)
                                //    {
                                //        if (PromptIndex(data.PromptsUsed, getStepScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                //        {
                                //            avgStpFre[k] = 1;
                                //        }

                                //        else
                                //        {
                                //            avgStpFre[k] = 0;
                                //        }
                                //    }
                                //}


                            }
                            else
                            {
                                avgCurrent[k] = -9999;
                                avgInd[k] = -9999;
                                //avgIndAll[k] = -9999;


                            }

                            if (data.PromptExcludeCrntStepMoveUp.TotalTrial > 0 || data.ExcludeCrntStepMoveUp.TotalTrial > 0 || data.SetExcludeCrntStepMoveUp.TotalTrial > 0)
                            {
                                if (data.next == data.StepCount)
                                {
                                    data.next = data.next - 1;
                                }
                                if (data.next == 0)
                                {
                                    data.next = 1;
                                }

                                if ((data.Sessions[k].Steps[j].Score != null) && (data.Sessions[k].Steps[j].Score.ToString() != ""))
                                {
                                    if (data.PromptsUsed.Length > 0)
                                    {
                                        if (avgCorrect[k] + avgIncorrect[k] == 1 || avgCorrect[k] + avgIncorrect[k] == 0)
                                        {
                                            if ((data.Sessions[k].Steps[data.next - 1].Score != null) && (data.Sessions[k].Steps[data.next - 1].Score.ToString() != ""))
                                            {
                                                avgExcludeCrnt[k] = 0;
                                            }
                                            else
                                            {

                                                if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                                {
                                                    avgExcludeCrnt[k] = 1;
                                                }

                                                else
                                                {
                                                    avgExcludeCrnt[k] = 0;
                                                }

                                            }

                                        }
                                        else if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                        {
                                            avgExcludeCrnt[k] = 1;
                                        }

                                        else
                                        {
                                            if (j == data.next - 1)
                                            {
                                                avgExcludeCrnt[k] = 1;
                                            }
                                            else
                                            {
                                                avgExcludeCrnt[k] = 0;
                                            }
                                        }


                                    }
                                }
                                else
                                {
                                    if (data.PromptsUsed.Length > 0)
                                    {

                                        avgExcludeCrnt[k] = 1;
                                    }


                                }
                            }
                        }
                        else if (data.Sessions[k].Steps[j].Score.Contains(":") && data.PromptsUsed.Length > 0 && (data.StepAvgDurationMoveUp.TotalTrial > 0 || data.StepTotalDurationMoveUp.TotalTrial > 0))
                        {
                            if (data.StepAvgDurationMoveUp.TotalTrial > 0)
                            {
                                int GetStat = data.FreDurMoveStat;
                                string getStepScore = "";
                                double getTime = TimeSpan.Parse(data.Sessions[k].Steps[data.CurrentStep - 1].Score).TotalSeconds;

                                if (GetStat == 1)
                                {
                                    if (Convert.ToInt32(getTime) < data.StepAvgDurationMoveUp.BarCondition)
                                    {
                                        getStepScore = "+";
                                    }
                                    else
                                    {
                                        getStepScore = "-";
                                    }
                                }
                                else if (GetStat == 0)
                                {
                                    if (Convert.ToInt32(getTime) > data.StepAvgDurationMoveUp.BarCondition)
                                    {
                                        getStepScore = "+";
                                    }
                                    else
                                    {
                                        getStepScore = "-";
                                    }
                                }

                                if (data.PromptsUsed.Length > 0)
                                {
                                    float avgdur = (float)getTime;
                                    avgStpAvgDurNew[k] = avgdur;
                                    if (PromptIndex(data.PromptsUsed, getStepScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                    {
                                        avgStpAvgDur[k] = 1;
                                    }
                                    else
                                    {
                                        avgStpAvgDur[k] = 0;
                                    }
                                }
                            }

                            if (data.StepTotalDurationMoveUp.TotalTrial > 0)
                            {
                                int GetStat = data.FreDurMoveStat;
                                string getStepScore = "";
                                double getTime = TimeSpan.Parse(data.Sessions[k].Steps[data.CurrentStep - 1].Score).TotalSeconds;
                               
                                if (GetStat == 1)
                                {
                                    //if (Convert.ToInt32(data.Sessions[k].Steps[j].Score) < data.StepTotalDurationMoveUp.BarCondition)
                                    if (Convert.ToInt32(getTime) < data.StepTotalDurationMoveUp.BarCondition)
                                    {
                                        getStepScore = "+";
                        }
                        else
                        {
                                        getStepScore = "-";
                                    }
                                }
                                else if (GetStat == 0)
                                {
                                    //if (Convert.ToInt32(data.Sessions[k].Steps[j].Score) > data.StepTotalDurationMoveUp.BarCondition)
                                    if (Convert.ToInt32(getTime) > data.StepTotalDurationMoveUp.BarCondition)

                                    {
                                        getStepScore = "+";
                                    }
                                    else
                                    {
                                        getStepScore = "-";
                                    }
                                }

                                if (data.PromptsUsed.Length > 0)
                                {
                                    float totdur = (float)getTime;
                                    avgStpTotDurNew[k] = totdur;
                                    if (PromptIndex(data.PromptsUsed, getStepScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                    {
                                        avgStpTotDur[k] = 1;
                                    }

                                    else
                                    {
                                        avgStpTotDur[k] = 0;
                                    }
                                }
                            }
                           
                        }
                        else
                        {
                            bSuccess = false;
                            bSuccessStep = false;
                        }

                    }
                    //float[] avgExcludeCrnt = new float[data.Sessions.Length];
                    //float CrctRespCount = 0;
                    //int stepCount = 0;
                    //for (int k = 0; k < data.Sessions.Length; k++)
                    //{
                    //    for (int index = 0; index < data.Sessions[k].Steps.Length; index++)
                    //    {
                    //        if (data.Sessions[k].Steps[index] != data.Sessions[k].Steps[data.CurrentStep - 1])
                    //        {
                    //            if (data.Sessions[k].Steps[index].Score == data.CorrectResp)
                    //            {
                    //                CrctRespCount++;
                    //            }
                    //            stepCount++;
                    //        }
                    //        else break;
                    //    }
                    //    avgExcludeCrnt[k] = CrctRespCount / stepCount;
                    //    CrctRespCount = 0; stepCount = 0;
                    //}

                    if (data.StepFrequencyMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepFrequencyMoveUp|--(Start)-- //
                        if (data.StepFrequencyMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgStpFreNew, data.StepFrequencyMoveUp.BarCondition, true, data.StepFrequencyMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.StepFrequencyMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepFrequencyMoveUp|--(End)-- //
                        else if (data.StepFrequencyMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgStpFre, data.StepFrequencyMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.StepFrequencyMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                           
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgStpFre, data.StepFrequencyMoveUp.BarCondition, data.StepFrequencyMoveUp.TotalTrial, true);
                            if (AccCount < data.StepFrequencyMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }

                    if (data.StepAvgDurationMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepAvgDurationMoveUp|--(Start)-- //
                        if (data.StepAvgDurationMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgStpAvgDurNew, data.StepAvgDurationMoveUp.BarCondition, true, data.StepAvgDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.StepAvgDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepAvgDurationMoveUp|--(End)-- //
                        else if (data.StepAvgDurationMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgStpAvgDur, data.StepAvgDurationMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.StepAvgDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgStpAvgDur, data.StepAvgDurationMoveUp.BarCondition, data.StepAvgDurationMoveUp.TotalTrial, true);
                            if (AccCount < data.StepAvgDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }

                    if (data.StepTotalDurationMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepTotalDurationMoveUp|--(Start)-- //
                        if (data.StepTotalDurationMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgStpTotDurNew, data.StepTotalDurationMoveUp.BarCondition, true, data.StepTotalDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.StepTotalDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepTotalDurationMoveUp|--(End)-- //
                        else if (data.StepTotalDurationMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgStpTotDur, data.StepTotalDurationMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.StepTotalDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgStpTotDur, data.StepTotalDurationMoveUp.BarCondition, data.StepTotalDurationMoveUp.TotalTrial, true);
                            if (AccCount < data.StepTotalDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }

                    if (data.StepPercentAccuracy.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepPercentAccuracy|--(Start)-- //
                        if (data.StepPercentAccuracy.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgCurrent, data.StepPercentAccuracy.BarCondition, true, data.StepPercentAccuracy.TotalTrial);
                            if (AccConsecutiveCount < data.StepPercentAccuracy.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepPercentAccuracy|--(End)-- //
                        else if (data.StepPercentAccuracy.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgCurrent, data.StepPercentAccuracy.BarCondition, true);
                            if (AccConsecutiveCount < data.StepPercentAccuracy.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgCurrent, data.StepPercentAccuracy.BarCondition, data.StepPercentAccuracy.TotalTrial, true);
                            if (AccCount < data.StepPercentAccuracy.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }
                    if (data.StepPercentIndependence.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepPercentIndependence|--(Start)-- //
                        if (data.StepPercentIndependence.ConsecutiveAverage)
                        {
                            int IndConsecutiveCount = ConsecutiveCountTestNew(avgInd, data.StepPercentIndependence.BarCondition, true, data.StepPercentIndependence.TotalTrial);
                            if (IndConsecutiveCount < data.StepPercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepPercentIndependence|--(End)-- //
                        else if (data.StepPercentIndependence.ConsecutiveSuccess)
                        {
                            int IndConsecutiveCount = ConsecutiveCount(avgInd, data.StepPercentIndependence.BarCondition, true);
                            if (IndConsecutiveCount < data.StepPercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int IndCount = SuccessORFailureCount(avgInd, data.StepPercentIndependence.BarCondition, data.StepPercentIndependence.TotalTrial, true);
                            if (IndCount < data.StepPercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }

                    //%independent of all steps
                    //if (data.StepPercentAllIndependence.TotalTrial > 0)
                    //{
                    //    if (data.StepPercentAllIndependence.ConsecutiveSuccess)
                    //    {
                    //        int IndConsecutiveCount = ConsecutiveCount(avgIndAll, data.StepPercentAllIndependence.BarCondition, true);
                    //        if (IndConsecutiveCount < data.StepPercentAllIndependence.SuccessNeeded)
                    //        {
                    //            bSuccess = false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        int IndCount = SuccessORFailureCount(avgIndAll, data.StepPercentAllIndependence.BarCondition, data.StepPercentAllIndependence.TotalTrial, true);
                    //        if (IndCount < data.StepPercentAllIndependence.SuccessNeeded)
                    //        {
                    //            bSuccess = false;
                    //        }
                    //    }
                    //}
                    if (data.LearnedStepMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|LearnedStepMoveUp|--(Start)-- //
                        if (data.LearnedStepMoveUp.ConsecutiveAverage)
                        {
                            int LearnedConsecutiveCount = ConsecutiveCountTestNew(avgLearned, data.LearnedStepMoveUp.BarCondition, true, data.LearnedStepMoveUp.TotalTrial);
                            if (LearnedConsecutiveCount < data.LearnedStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|LearnedStepMoveUp|--(End)-- //
                        else if (data.LearnedStepMoveUp.ConsecutiveSuccess)
                        {
                            int LearnedConsecutiveCount = ConsecutiveCount(avgLearned, data.LearnedStepMoveUp.BarCondition, true);
                            if (LearnedConsecutiveCount < data.LearnedStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int LearnedCount = SuccessORFailureCount(avgLearned, data.LearnedStepMoveUp.BarCondition, data.LearnedStepMoveUp.TotalTrial, true);
                            if (LearnedCount < data.LearnedStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }

                    if (data.ExcludeCrntStepMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|ExcludeCrntStepMoveUp|--(Start)-- //
                        if (data.ExcludeCrntStepMoveUp.ConsecutiveAverage)
                        {
                            int LearnedConsecutiveCount = ConsecutiveCountTestNew(avgExcludeCrnt, data.ExcludeCrntStepMoveUp.BarCondition, true, data.ExcludeCrntStepMoveUp.TotalTrial);
                            if (LearnedConsecutiveCount < data.ExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|ExcludeCrntStepMoveUp|--(End)-- //
                        else if (data.ExcludeCrntStepMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgExcludeCrnt, data.ExcludeCrntStepMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.ExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgExcludeCrnt, data.ExcludeCrntStepMoveUp.BarCondition, data.ExcludeCrntStepMoveUp.TotalTrial, true);
                            if (AccCount < data.ExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }
                    if (data.StepTotalCorrectMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepTotalCorrectMoveUp|--(Start)-- //
                        if (data.StepTotalCorrectMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewCorrect(avgCorrect, data.StepTotalCorrectMoveUp.BarCondition, true, data.StepTotalCorrectMoveUp.TotalTrial);
                            if (AccConsecutiveCount < data.StepTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepTotalCorrectMoveUp|--(End)-- //
                        else if (data.StepTotalCorrectMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, data.StepTotalCorrectMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.StepTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountCorrect(avgCorrect, data.StepTotalCorrectMoveUp.BarCondition, data.StepTotalCorrectMoveUp.TotalTrial, true);
                            if (AccCount < data.StepTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }
                    if (data.PromptLearnedStepMoveUp.TotalTrial == 0 && data.StepPercentIndependence.TotalTrial == 0 && data.LearnedStepMoveUp.TotalTrial == 0 && data.StepPercentAccuracy.TotalTrial == 0 && data.ExcludeCrntStepMoveUp.TotalTrial == 0 && data.StepFrequencyMoveUp.TotalTrial == 0 && data.StepAvgDurationMoveUp.TotalTrial == 0 && data.StepTotalDurationMoveUp.TotalTrial == 0 && data.StepTotalCorrectMoveUp.TotalTrial ==0 )
                    {
                        bSuccess = false;
                        bSuccessStep = false;
                    }
                    //if (data.ExcludeCrntStepMoveUp.BarCondition > 0)
                    //{
                    //    if (data.ExcludeCrntStepMoveUp.ConsecutiveSuccess)
                    //    {
                    //        int ExcludeConsecutiveCount = ConsecutiveCount(avgExcludeCrnt, data.ExcludeCrntStepMoveUp.BarCondition, true);
                    //        if (ExcludeConsecutiveCount < data.ExcludeCrntStepMoveUp.SuccessNeeded)
                    //        {
                    //            bSuccess = false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        int ExcludeCount = SuccessORFailureCount(avgExcludeCrnt, data.ExcludeCrntStepMoveUp.BarCondition, true);
                    //        if (ExcludeCount < data.ExcludeCrntStepMoveUp.SuccessNeeded)
                    //        {
                    //            bSuccess = false;
                    //        }
                    //    }
                    //}

                    if (data.multiMeasure == true)
                    {
                        if (!bSuccessStep && !bSuccessPrompt)
                        {
                            bSuccess = true;
                            if (data.PercentAccuracy.TotalTrial == 0 && data.PercentIndependence.TotalTrial == 0 && data.FrequencyMoveUp.TotalTrial == 0 && data.AvgDurationMoveUp.TotalTrial == 0 && data.TotalDurationMoveUp.TotalTrial == 0)
                            {
                                bSuccess = false;
                            }
                        }
                        else if (!bSuccessStep && bSuccessPrompt)
                        {
                            bSuccess = true;
                            if (data.PercentAccuracy.TotalTrial == 0 && data.PercentIndependence.TotalTrial == 0 && data.FrequencyMoveUp.TotalTrial == 0 && data.AvgDurationMoveUp.TotalTrial == 0 && data.TotalDurationMoveUp.TotalTrial == 0)
                            {
                                bSuccess = false;
                            }
                        }
                        //Step bSuccess comes fine by default.(while enable this code step criteria not meeting)
                        //else if (bSuccessStep && !bSuccessPrompt)
                        //{
                        //    bSuccess = true;
                        //    if (data.PercentAccuracy.TotalTrial == 0 && data.PercentIndependence.TotalTrial == 0)
                        //    {
                        //        bSuccess = false;
                        //    }
                        //}
                    }

                    if (bSuccess)
                        bAtleastOneSuccess = true;

                }
                if (bAtleastOneSuccess)
                {
                    //Goto Next step
                    res.NextStep = j + 1 + 1;
                    //res.NextPrompt = data.PromptsUsed[0];
                    return res;
                }
                else
                {
                    //Return back with current step
                    res.NextStep = j + 1;
                    res.NextPrompt = data.TargetPrompt;
                    return res;
                }
            }

            res.NextStep = int.MaxValue;
            res.NextPrompt = "*";

            return res;

        }

        public static int GetNextStep(InputData data)
        {

            int iStartIndex = PromptIndex(data.PromptsUsed, data.CurrentPrompt);
            int iStartStep = data.CurrentStep;
            int curr = data.next;
            int marked_count1 = data.marked;
            bool bsucs = true;
            float[] avgCorrect = new float[data.Sessions.Length];
            float[] avgIncorrect = new float[data.Sessions.Length];
            for (int i = 0; i < data.TrialsData.Length; i++)
            {
                avgCorrect[i] = data.TrialsData[i].Correct;
                avgIncorrect[i] = data.TrialsData[i].Incorrect;
            }
            for (int j = iStartStep; j < data.Sessions[0].Steps.Length; j++)
            {
                bool bSuccess = false;
                for (int i = iStartIndex; i < data.PromptsUsed.Length; i++)
                {
                    bSuccess = true;
                    float[] avgAcc = new float[data.Sessions.Length];
                    float[] avgInd = new float[data.Sessions.Length];
                    float[] avgIndAll = new float[data.Sessions.Length];
                    float[] avgLearned = new float[data.Sessions.Length];
                    float[] avgExcludeCrnt = new float[data.Sessions.Length];
                    float[] avgStpFre = new float[data.Sessions.Length];
                    float[] avgStpAvgDur = new float[data.Sessions.Length];
                    float[] avgStpTotDur = new float[data.Sessions.Length];
                    //float CrctRespCount = 0;
                    //int stepCount = 0;
                    float CrctRespCountat = 0;
                    for (int k = 0; k < data.Sessions.Length; k++)
                    {
                        if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                        {
                            avgAcc[k] = 1;
                        }
                        else
                        {
                            avgAcc[k] = 0;
                        }
                        if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) == (data.PromptsUsed.Length - 1))
                        {
                            avgInd[k] = 1;
                        }
                        else
                        {
                            avgInd[k] = 0;
                        }
                        //%independent of all steps
                        if (data.PercentAllIndependence.TotalTrial > 0 || data.StepPercentAllIndependence.TotalTrial > 0 || data.PromptPercentAllIndependence.TotalTrial > 0)
                        {
                            if (data.Sessions[k].Steps[j].Score == "+")
                            {
                                if (data.sCurrentLessonPrompt == "118")
                                {
                                    avgIndAll[k] = 1;
                                }
                                else
                                {
                                    avgIndAll[k] = 0;
                                }
                            }
                            else
                            {
                                if (data.Sessions[k].Steps[j].Score == "118")
                                {
                                    avgIndAll[k] = 1;
                                }
                                else
                                {
                                    avgIndAll[k] = 0;
                                }
                            }
                        }
                        if (data.PromptExcludeCrntStepMoveUp.TotalTrial > 0 || data.ExcludeCrntStepMoveUp.TotalTrial > 0 || data.SetExcludeCrntStepMoveUp.TotalTrial > 0)
                        {
                            if (data.next == data.StepCount)
                            {
                                data.next = data.next - 1;
                            }
                            if (data.next == 0)
                            {
                                data.next = 1;
                            }
                            if ((data.Sessions[k].Steps[j].Score != null) && (data.Sessions[k].Steps[j].Score.ToString() != ""))
                            {
                                if (data.PromptsUsed.Length > 0)
                                {
                                    if (avgCorrect[k] + avgIncorrect[k] == 1 || avgCorrect[k] + avgIncorrect[k] == 0)
                                    {
                                        if ((data.Sessions[k].Steps[data.next - 1].Score != null) && (data.Sessions[k].Steps[data.next - 1].Score.ToString() != ""))
                                        {
                                            avgExcludeCrnt[k] = 0;
                                        }
                                        else
                                        {

                                            if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                            {
                                                avgExcludeCrnt[k] = 1;
                                            }

                                            else
                                            {
                                                if (j == data.next - 1)
                                                {
                                                    avgExcludeCrnt[k] = 1;
                                                }
                                                else
                                                {
                                                    avgExcludeCrnt[k] = 0;
                                                }
                                            }

                                        }

                                    }
                                    else if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                    {
                                        avgExcludeCrnt[k] = 1;
                                    }

                                    else
                                    {
                                        avgExcludeCrnt[k] = 0;
                                    }


                                }
                            }
                            else
                            {
                                if (data.PromptsUsed.Length > 0)
                                {

                                    avgExcludeCrnt[k] = 1;
                                }


                            }
                        }


                        //train
                        if (data.PromptLearnedStepMoveUp.TotalTrial > 0 || data.LearnedStepMoveUp.TotalTrial > 0 || data.SetLearnedStepMoveUp.TotalTrial > 0)
                        {
                            if (data.next == data.StepCount)
                            {
                                data.next = data.next - 1;
                            }
                            if (data.next == 0)
                            {
                                data.next = 1;
                            }
                            if (data.PromptsUsed.Length > 0)
                            {
                                if (data.Sessions[k].Steps[data.next - 1].Score == data.CorrectResp)
                                {
                                    CrctRespCountat++;
                                }
                            }
                            avgLearned[k] = CrctRespCountat;
                            CrctRespCountat = 0;
                        }


                    }

                    if (data.StepFrequencyMoveUp.BarCondition > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepFrequencyMoveUp|--(Start)-- //
                        if (data.StepFrequencyMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgStpFre, data.StepFrequencyMoveUp.BarCondition, true, data.StepFrequencyMoveUp.TotalTrial);
                            if (AccConsecutiveCount < data.StepFrequencyMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepFrequencyMoveUp|--(End)-- //
                        else if (data.StepFrequencyMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgStpFre, data.StepFrequencyMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.StepFrequencyMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgStpFre, data.StepFrequencyMoveUp.BarCondition, data.StepFrequencyMoveUp.TotalTrial, true);
                            if (AccCount < data.StepFrequencyMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }

                    if (data.StepAvgDurationMoveUp.BarCondition > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepAvgDurationMoveUp|--(Start)-- //
                        if (data.StepAvgDurationMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgStpAvgDur, data.StepAvgDurationMoveUp.BarCondition, true, data.StepAvgDurationMoveUp.TotalTrial);
                            if (AccConsecutiveCount < data.StepAvgDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepAvgDurationMoveUp|--(End)-- //
                        else if (data.StepAvgDurationMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgStpAvgDur, data.StepAvgDurationMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.StepAvgDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgStpAvgDur, data.StepAvgDurationMoveUp.BarCondition, data.StepAvgDurationMoveUp.TotalTrial, true);
                            if (AccCount < data.StepAvgDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }

                    if (data.StepTotalDurationMoveUp.BarCondition > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepTotalDurationMoveUp|--(Start)-- //
                        if (data.StepTotalDurationMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgStpTotDur, data.StepTotalDurationMoveUp.BarCondition, true, data.StepTotalDurationMoveUp.TotalTrial);
                            if (AccConsecutiveCount < data.StepTotalDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepTotalDurationMoveUp|--(End)-- //
                        else if (data.StepTotalDurationMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgStpTotDur, data.StepTotalDurationMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.StepTotalDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgStpTotDur, data.StepTotalDurationMoveUp.BarCondition, data.StepTotalDurationMoveUp.TotalTrial, true);
                            if (AccCount < data.StepTotalDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }

                    if (data.StepPercentAccuracy.BarCondition > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepPercentAccuracy|--(Start)-- //
                        if (data.StepPercentAccuracy.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgAcc, data.StepPercentAccuracy.BarCondition, true, data.StepPercentAccuracy.TotalTrial);
                            if (AccConsecutiveCount < data.StepPercentAccuracy.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepPercentAccuracy|--(End)-- //
                        else if (data.StepPercentAccuracy.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgAcc, data.StepPercentAccuracy.BarCondition, true);
                            if (AccConsecutiveCount < data.StepPercentAccuracy.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgAcc, data.StepPercentAccuracy.BarCondition, data.StepPercentAccuracy.TotalTrial, true);
                            if (AccCount < data.StepPercentAccuracy.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }
                    if (data.StepPercentIndependence.BarCondition > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepPercentIndependence|--(Start)-- //
                        if (data.StepPercentIndependence.ConsecutiveAverage)
                        {
                            int IndConsecutiveCount = ConsecutiveCountTestNew(avgInd, data.StepPercentIndependence.BarCondition, true, data.StepPercentIndependence.TotalTrial);
                            if (IndConsecutiveCount < data.StepPercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepPercentIndependence|--(End)-- //
                        else if (data.StepPercentIndependence.ConsecutiveSuccess)
                        {
                            int IndConsecutiveCount = ConsecutiveCount(avgInd, data.StepPercentIndependence.BarCondition, true);
                            if (IndConsecutiveCount < data.StepPercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int IndCount = SuccessORFailureCount(avgInd, data.StepPercentIndependence.BarCondition, data.StepPercentIndependence.TotalTrial, true);
                            if (IndCount < data.StepPercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }

                    }
                    //independent of all steps
                    if (data.StepPercentAllIndependence.BarCondition > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepPercentAllIndependence|--(Start)-- //
                        if (data.StepPercentAllIndependence.ConsecutiveAverage)
                        {
                            int IndConsecutiveCount = ConsecutiveCountTestNew(avgInd, data.StepPercentAllIndependence.BarCondition, true, data.StepPercentAllIndependence.TotalTrial);
                            if (IndConsecutiveCount < data.StepPercentAllIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepPercentAllIndependence|--(End)-- //
                        else if (data.StepPercentAllIndependence.ConsecutiveSuccess)
                        {
                            int IndConsecutiveCount = ConsecutiveCount(avgIndAll, data.StepPercentAllIndependence.BarCondition, true);
                            if (IndConsecutiveCount < data.StepPercentAllIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int IndCount = SuccessORFailureCount(avgIndAll, data.StepPercentAllIndependence.BarCondition, data.StepPercentAllIndependence.TotalTrial, true);
                            if (IndCount < data.StepPercentAllIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }

                    }
                    if (data.StepTotalCorrectMoveUp.BarCondition > 0)
                    {
                        //--- [New Criteria] May 2020 --|StepTotalCorrectMoveUp|--(Start)-- //
                        if (data.StepPercentAllIndependence.ConsecutiveAverage)
                        {
                            int TCConsecutiveCount = ConsecutiveCountTestNew(avgInd, data.StepTotalCorrectMoveUp.BarCondition, true, data.StepTotalCorrectMoveUp.TotalTrial);
                            if (TCConsecutiveCount < data.StepTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|StepTotalCorrectMoveUp|--(End)-- //
                        else if (data.StepTotalCorrectMoveUp.ConsecutiveSuccess)
                        {
                            int TCConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, data.StepTotalCorrectMoveUp.BarCondition, true);
                            if (TCConsecutiveCount < data.StepTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int TCCount = SuccessORFailureCountCorrect(avgCorrect, data.StepTotalCorrectMoveUp.BarCondition, data.StepTotalCorrectMoveUp.TotalTrial, true);
                            if (TCCount < data.StepTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }
                    if (data.LearnedStepMoveUp.BarCondition > 0)
                    {
                        //--- [New Criteria] May 2020 --|LearnedStepMoveUp|--(Start)-- //
                        if (data.LearnedStepMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgInd, data.LearnedStepMoveUp.BarCondition, true, data.LearnedStepMoveUp.TotalTrial);
                            if (AccConsecutiveCount < data.LearnedStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|LearnedStepMoveUp|--(End)-- //
                        else if (data.LearnedStepMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgLearned, data.LearnedStepMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.LearnedStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgLearned, data.LearnedStepMoveUp.BarCondition, data.LearnedStepMoveUp.TotalTrial, true);
                            if (AccCount < data.LearnedStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }


                    if (data.ExcludeCrntStepMoveUp.BarCondition > 0)
                    {
                        //--- [New Criteria] May 2020 --|ExcludeCrntStepMoveUp|--(Start)-- //
                        if (data.ExcludeCrntStepMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgInd, data.ExcludeCrntStepMoveUp.BarCondition, true, data.ExcludeCrntStepMoveUp.TotalTrial);
                            if (AccConsecutiveCount < data.ExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        } //--- [New Criteria] May 2020 --|ExcludeCrntStepMoveUp|--(End)-- //
                        else if (data.ExcludeCrntStepMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgExcludeCrnt, data.ExcludeCrntStepMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.ExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                                bsucs = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgExcludeCrnt, data.ExcludeCrntStepMoveUp.BarCondition, data.ExcludeCrntStepMoveUp.TotalTrial, true);
                            if (AccCount < data.ExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                                bsucs = false;
                            }
                        }
                    }

                    if (bSuccess)
                        return j;
                }

            }

            return int.MaxValue;
        }
        public static bool CheckMoveBack(InputData data, MoveBackCriteria criteria, string sCheckFor, bool bIndFlag)
        {
            int iStartIndex = PromptIndex(data.PromptsUsed, sCheckFor);
            for (int i = iStartIndex; i < data.PromptsUsed.Length; i++)
            {
                float[] avgAcc = new float[data.Sessions.Length];
                float[] avgInd = new float[data.Sessions.Length];

                for (int k = 0; k < data.Sessions.Length; k++)
                {
                    if ((data.Sessions[k].Steps[data.CurrentStep - 1].Score != null) && (data.Sessions[k].Steps[data.CurrentStep - 1].Score.ToString() != ""))
                    {
                        if (bIndFlag)
                        {
                            // avgAcc[k] = data.Sessions[k].Steps[data.CurrentStep - 1].PromptAverages[data.PromptsUsed[data.PromptsUsed.Length - 1]];
                            if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[data.CurrentStep - 1].Score) == (data.PromptsUsed.Length - 1))
                            {
                                avgAcc[k] = 1;
                            }
                            else
                            {
                                avgAcc[k] = 0;
                            }
                        }
                        else
                        {
                            //avgAcc[k] = data.Sessions[k].Steps[data.CurrentStep - 1].PromptAverages[data.PromptsUsed[i]];
                            if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[data.CurrentStep - 1].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                            {
                                avgAcc[k] = 1;
                            }
                            else
                            {
                                avgAcc[k] = 0;
                            }
                        }
                    }
                    else
                    {
                        avgAcc[k] = -9999;
                    }
                }


                if (criteria.TotalTrial > 0)
                {
                    //--- [New Criteria] May 2020 --|avgAcc|--(Start)-- //
                    if (criteria.ConsecutiveAverageFailure)
                    {
                        int AccConsecutiveCount = ConsecutiveCountTestNew(avgAcc, criteria.BarCondition, false,criteria.TotalTrial);                        
                        if (AccConsecutiveCount < criteria.FailureNeeded)
                            return false;
                    } //--- [New Criteria] May 2020 --|avgAcc|--(End)-- //
                    else if (criteria.ConsecutiveFailures)
                    {
                        int AccConsecutiveCount = ConsecutiveCount(avgAcc, criteria.BarCondition, false);
                        if (AccConsecutiveCount < criteria.FailureNeeded)
                            return false;
                    }
                    else
                    {
                        int AccCount = SuccessORFailureCount(avgAcc, criteria.BarCondition, criteria.TotalTrial, false);
                        if (AccCount < criteria.FailureNeeded)
                            return false;
                    }
                }
            }
            return true;
        }
        public static bool CheckMoveBackCorrect(InputData data, MoveBackCriteria criteria, string sCheckFor, bool bIndFlag, bool IsTotalCorrect)
        {
            float[] avgCorrect = new float[data.Sessions.Length];
            float[] avgIncorrect = new float[data.Sessions.Length];
            int AccConsecutiveCount, AccCount;
            for (int i = 0; i < data.TrialsData.Length; i++)
            {
                avgCorrect[i] = data.TrialsData[i].Correct;
                avgIncorrect[i] = data.TrialsData[i].Incorrect;
            }
            if (criteria.TotalTrial > 0)
            {

                //--- [New Criteria] May 2020 ---(Start)-- //
                if (criteria.ConsecutiveAverageFailure)
                {
                    if (IsTotalCorrect)
                    {
                        AccConsecutiveCount = ConsecutiveCountTestNewCorrect(avgCorrect, criteria.BarCondition, false, criteria.TotalTrial);
                    }
                    else
                    {
                        AccConsecutiveCount = ConsecutiveCountTestNewCorrect(avgIncorrect, criteria.BarCondition, true, criteria.TotalTrial);
                    }
                    if (AccConsecutiveCount < criteria.FailureNeeded)
                    {
                        return false;
                    }
                } //--- [New Criteria] May 2020 ---(End)-- //
                else if (criteria.ConsecutiveFailures)
                {
                    if (IsTotalCorrect)
                        AccConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, criteria.BarCondition, false);
                    else
                        AccConsecutiveCount = ConsecutiveCountCorrect(avgIncorrect, criteria.BarCondition, false);
                    if (AccConsecutiveCount < criteria.FailureNeeded)
                        return false;
                }
                else
                {
                    if (IsTotalCorrect)
                        AccCount = SuccessORFailureCountCorrect(avgCorrect, criteria.BarCondition, criteria.TotalTrial, false);
                    else
                        AccCount = SuccessORFailureCountCorrect(avgIncorrect, criteria.BarCondition, criteria.TotalTrial, false);
                    if (AccCount < criteria.FailureNeeded)
                        return false;
                }
            }
            return true;
        }
        public static bool CheckMoveBack1(InputData data, MoveBackCriteria criteria, string sCheckFor, bool bIndFlag)
        {


            int iStartIndex = PromptIndex(data.PromptsUsed, sCheckFor);
            for (int i = iStartIndex; i < data.PromptsUsed.Length; i++)
            {
                float[] avgLearned = new float[data.Sessions.Length];
                //float CrctRespCountat = 0;

                for (int k = 0; k < data.Sessions.Length; k++)
                {
                    if ((data.Sessions[k].Steps[data.CurrentStep - 1].Score != null) && (data.Sessions[k].Steps[data.CurrentStep - 1].Score.ToString() != ""))
                    {
                        //if (data.PromptLearnedStepMoveUp.TotalTrial > 0)
                        //{
                        if (data.PromptsUsed.Length > 0)
                        {
                            if (data.next == data.StepCount)
                            {
                                data.next = data.next - 1;
                            }
                            if (data.next == 0)
                            {
                                data.next = 1;
                            }
                            if (data.Sessions[k].Steps[data.next - 1].Score == data.CorrectResp)
                            {
                                avgLearned[k] = 1;
                            }
                            else
                            {
                                if (data.Sessions[k].Steps[data.CurrentStep - 1] != data.Sessions[k].Steps[data.next - 1])
                                {
                                    avgLearned[k] = 1;
                                }
                                else
                                {
                                    avgLearned[k] = 0;
                                }
                            }
                        }
                        //}

                    }
                    else
                    {
                        avgLearned[k] = 1;
                    }


                }
                if (criteria.TotalTrial > 0)
                {
                    //--- [New Criteria] May 2020 --|avgLearned|--(Start)-- //
                    if (criteria.ConsecutiveAverageFailure)
                    {
                        int AccConsecutiveCount = ConsecutiveCountTestNew(avgLearned, criteria.BarCondition, false, criteria.TotalTrial);
                        if (AccConsecutiveCount < criteria.FailureNeeded)
                            return false;
                    } //--- [New Criteria] May 2020 --|avgLearned|--(End)-- //
                    else if (criteria.ConsecutiveFailures)
                    {
                        int AccConsecutiveCount = ConsecutiveCount(avgLearned, criteria.BarCondition, false);
                        if (AccConsecutiveCount < criteria.FailureNeeded)
                            return false;
                    }
                    else
                    {
                        int AccCount = SuccessORFailureCount(avgLearned, criteria.BarCondition, criteria.TotalTrial, false);
                        if (AccCount < criteria.FailureNeeded)
                            return false;
                    }
                }
            }
            return true;
        }


        public static bool CheckMoveBack2(InputData data, MoveBackCriteria criteria, string sCheckFor, bool bIndFlag)
        {
            bool bsucs = true;
            int curr = data.next;
            int marked_count1 = data.marked;
            float[] avgCorrect = new float[data.Sessions.Length];
            float[] avgIncorrect = new float[data.Sessions.Length];

            for (int i = 0; i < data.TrialsData.Length; i++)
            {
                avgCorrect[i] = data.TrialsData[i].Correct;
                avgIncorrect[i] = data.TrialsData[i].Incorrect;
            }

            int iStartIndex = PromptIndex(data.PromptsUsed, sCheckFor);
            for (int i = iStartIndex; i < data.PromptsUsed.Length; i++)
            {
                float[] avgExcludeCrnt = new float[data.Sessions.Length];
                //float CrctRespCount = 0;
                //int stepCount = 0;


                for (int k = 0; k < data.Sessions.Length; k++)
                {
                    if ((data.Sessions[k].Steps[data.CurrentStep - 1].Score != null) && (data.Sessions[k].Steps[data.CurrentStep - 1].Score.ToString() != ""))
                    {
                        if (data.next == data.StepCount)
                        {
                            data.next = data.next - 1;
                        }
                        if (data.next == 0)
                        {
                            data.next = 1;
                        }
                        if (data.Sessions[k].Steps[data.next - 1] != data.Sessions[k].Steps[data.CurrentStep - 1])
                        {

                            if (bIndFlag)
                            {
                                // avgAcc[k] = data.Sessions[k].Steps[data.CurrentStep - 1].PromptAverages[data.PromptsUsed[data.PromptsUsed.Length - 1]];
                                if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[data.CurrentStep - 1].Score) == (data.PromptsUsed.Length - 1))
                                {
                                    avgExcludeCrnt[k] = 1;
                                }
                                else
                                {
                                    avgExcludeCrnt[k] = 0;
                                }
                            }
                            else
                            {
                                //avgAcc[k] = data.Sessions[k].Steps[data.CurrentStep - 1].PromptAverages[data.PromptsUsed[i]];
                                if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[data.CurrentStep - 1].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                {
                                    avgExcludeCrnt[k] = 1;
                                }
                                else
                                {
                                    avgExcludeCrnt[k] = 0;
                                }
                            }
                        }
                        else
                        {
                            avgExcludeCrnt[k] = 1;
                        }

                    }
                    else
                    {
                        avgExcludeCrnt[k] = 1;
                    }





                }
                if (criteria.TotalTrial > 0)
                {
                    //--- [New Criteria] May 2020 --|avgExcludeCrnt|--(Start)-- //
                    if (criteria.ConsecutiveAverageFailure)
                    {
                        int AccConsecutiveCount = ConsecutiveCountTestNew(avgExcludeCrnt, criteria.BarCondition, false, criteria.TotalTrial);
                        if (AccConsecutiveCount < criteria.FailureNeeded)
                        {
                            return false;
                            bsucs = false;
                        }
                    } //--- [New Criteria] May 2020 --|avgExcludeCrnt|--(End)-- //
                    else if (criteria.ConsecutiveFailures)
                    {
                        int AccConsecutiveCount = ConsecutiveCount(avgExcludeCrnt, criteria.BarCondition, false);
                        if (AccConsecutiveCount < criteria.FailureNeeded)
                        {
                            return false;
                            bsucs = false;
                        }
                    }
                    else
                    {
                        int AccCount = SuccessORFailureCount(avgExcludeCrnt, criteria.BarCondition, criteria.TotalTrial, false);
                        if (AccCount < criteria.FailureNeeded)
                        {
                            return false;
                            bsucs = false;
                        }
                    }
                }


            }
            return true;
        }

        public static bool CheckMoveBackPer(InputData data, MoveBackCriteria criteria, string sCheckFor, bool bIndFlag)
        {
            int iStartIndex = PromptIndex(data.PromptsUsed, sCheckFor);
            for (int i = iStartIndex; i < data.PromptsUsed.Length; i++)
            {
                float[] avgInd = new float[data.Sessions.Length];

                for (int k = 0; k < data.Sessions.Length; k++)
                {
                    if ((data.Sessions[k].Steps[data.CurrentStep - 1].Score != null) && (data.Sessions[k].Steps[data.CurrentStep - 1].Score.ToString() != ""))
                    {
                        if (data.Sessions[k].Steps[data.CurrentStep - 1].Score == "+")
                        {
                            if (data.sCurrentLessonPrompt == "118")
                            {
                                avgInd[k] = 1;
                            }
                            else
                            {
                                avgInd[k] = 0;
                            }
                        }
                        else
                        {
                            if (data.Sessions[k].Steps[data.CurrentStep - 1].Score == "118")
                            {
                                avgInd[k] = 1;
                            }
                            else
                            {
                                avgInd[k] = 0;
                            }
                        }
                    }
                    else
                    {
                        avgInd[k] = 1;
                    }
                }


                if (criteria.TotalTrial > 0)
                {
                    //--- [New Criteria] May 2020 --|avgInd|--(Start)-- //
                    if (criteria.ConsecutiveAverageFailure)
                    {
                        int AccConsecutiveCount = ConsecutiveCountTestNew(avgInd, criteria.BarCondition, false, criteria.TotalTrial);
                        if (AccConsecutiveCount < criteria.FailureNeeded)                        
                            return false;
                    } //--- [New Criteria] May 2020 --|avgInd|--(End)-- //
                    else if (criteria.ConsecutiveFailures)
                    {
                        int AccConsecutiveCount = ConsecutiveCount(avgInd, criteria.BarCondition, false);
                        if (AccConsecutiveCount < criteria.FailureNeeded)
                            return false;
                    }
                    else
                    {
                        int AccCount = SuccessORFailureCount(avgInd, criteria.BarCondition, criteria.TotalTrial, false);
                        if (AccCount < criteria.FailureNeeded)
                            return false;
                    }
                }
            }
            return true;
        }

        public static bool CheckMoveBackFreDur(InputData data, MoveBackCriteria criteria, string sCheckFor, bool bIndFlag)
        {
            int iStartIndex = PromptIndex(data.PromptsUsed, sCheckFor);
            for (int i = iStartIndex; i < data.PromptsUsed.Length; i++)
            {
                float[] avgAcc = new float[data.Sessions.Length];
                float[] avgInd = new float[data.Sessions.Length];

                float[] avgAccNew = new float[data.Sessions.Length];



                for (int k = 0; k < data.Sessions.Length; k++)
                {
                    if ((data.Sessions[k].Steps[data.CurrentStep - 1].Score != null) && (data.Sessions[k].Steps[data.CurrentStep - 1].Score.ToString() != ""))
                    {
                        int GetStat = data.FreDurMoveStat;
                        string stepScore = "";

                        if (GetStat == 1)
                        {
                            if (Convert.ToInt32(data.Sessions[k].Steps[data.CurrentStep - 1].Score) <= criteria.BarCondition)
                            {
                                stepScore = "+";
                            }
                            else
                            {
                                stepScore = "-";
                            }
                        }
                        else if (GetStat == 0)
                        {
                            if (Convert.ToInt32(data.Sessions[k].Steps[data.CurrentStep - 1].Score) >= criteria.BarCondition)
                            {
                                stepScore = "+";
                            }
                            else
                            {
                                stepScore = "-";
                            }
                        }
                        avgAccNew[k]=(float)(Convert.ToInt32(data.Sessions[k].Steps[data.CurrentStep - 1].Score));
                        if (bIndFlag)
                        {
                           

                            // avgAcc[k] = data.Sessions[k].Steps[data.CurrentStep - 1].PromptAverages[data.PromptsUsed[data.PromptsUsed.Length - 1]];
                            if (PromptIndex(data.PromptsUsed, stepScore) == (data.PromptsUsed.Length - 1))
                            {
                                avgAcc[k] = 1;
                            }
                            else
                            {
                                avgAcc[k] = 0;
                            }
                        }
                        else
                        {
                            //avgAcc[k] = data.Sessions[k].Steps[data.CurrentStep - 1].PromptAverages[data.PromptsUsed[i]];
                            

                            if (PromptIndex(data.PromptsUsed, stepScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                            {
                                avgAcc[k] = 1;
                            }
                            else
                            {
                                avgAcc[k] = 0;
                            }
                        }
                    }
                    else
                    {
                        avgAcc[k] = -9999;
                    }
                }


                if (criteria.TotalTrial > 0)
                {
                    //--- [New Criteria] May 2020 --|avgAcc|--(Start)-- //
                    if (criteria.ConsecutiveAverageFailure)
                    {
                        int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgAccNew, criteria.BarCondition, false, criteria.TotalTrial, data.FreDurMoveStat);
                        if (AccConsecutiveCount < criteria.FailureNeeded)
                            return false;
                    } //--- [New Criteria] May 2020 --|avgAcc|--(End)-- //
                    else if (criteria.ConsecutiveFailures)
                    {
                        int AccConsecutiveCount = ConsecutiveCountfreqd(avgAcc, criteria.BarCondition, false);
                        if (AccConsecutiveCount < criteria.FailureNeeded)
                            return false;
                    }
                    else
                    {
                        int AccCount = SuccessORFailureCountFrequencyDuration(avgAcc, criteria.BarCondition, criteria.TotalTrial, false);
                        //if (AccCount < criteria.FailureNeeded)
                        //    return false;
                        if (criteria.FailureNeeded < criteria.TotalTrial)
                        {
                            if (AccCount < (criteria.FailureNeeded + 1))
                                return false;
                        }
                        else
                        {
                            if (AccCount < criteria.FailureNeeded)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        public static bool CheckMoveBackAvgTotDur(InputData data, MoveBackCriteria criteria, string sCheckFor, bool bIndFlag)
        {
            int iStartIndex = PromptIndex(data.PromptsUsed, sCheckFor);
            for (int i = iStartIndex; i < data.PromptsUsed.Length; i++)
            {
                float[] avgAcc = new float[data.Sessions.Length];
                float[] avgInd = new float[data.Sessions.Length];

                float[] avgAccNew = new float[data.Sessions.Length];

                for (int k = 0; k < data.Sessions.Length; k++)
                {
                    if ((data.Sessions[k].Steps[data.CurrentStep - 1].Score != null) && (data.Sessions[k].Steps[data.CurrentStep - 1].Score.ToString() != ""))
                    {
                        int GetStat = data.FreDurMoveStat;
                        string stepScore = "";
                        double getTime = TimeSpan.Parse(data.Sessions[k].Steps[data.CurrentStep - 1].Score).TotalSeconds;

                        if (GetStat == 1)
                        {
                            if (Convert.ToInt32(getTime) <= criteria.BarCondition)
                            {
                                stepScore = "+";
                            }
                            else
                            {
                                stepScore = "-";
                            }
                        }
                        else if (GetStat == 0)
                        {
                            if (Convert.ToInt32(getTime) >= criteria.BarCondition)
                            {
                                stepScore = "+";
                            }
                            else
                            {
                                stepScore = "-";
                            }
                        }
                        float avgdur = (float)getTime;
                        avgAccNew[k] = avgdur;

                        if (bIndFlag)
                        {
                            
                            // avgAcc[k] = data.Sessions[k].Steps[data.CurrentStep - 1].PromptAverages[data.PromptsUsed[data.PromptsUsed.Length - 1]];
                            if (PromptIndex(data.PromptsUsed, stepScore) == (data.PromptsUsed.Length - 1))
                            {
                                avgAcc[k] = 1;
                            }
                            else
                            {
                                avgAcc[k] = 0;
                            }
                        }
                        else
                        {
                            //avgAcc[k] = data.Sessions[k].Steps[data.CurrentStep - 1].PromptAverages[data.PromptsUsed[i]];
                            if (PromptIndex(data.PromptsUsed, stepScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                            {
                                avgAcc[k] = 1;
                            }
                            else
                            {
                                avgAcc[k] = 0;
                            }
                        }
                    }
                    else
                    {
                        avgAcc[k] = -9999;
                    }
                }


                if (criteria.TotalTrial > 0)
                {
                    //--- [New Criteria] May 2020 --|avgAcc|--(Start)-- //
                    if (criteria.ConsecutiveAverageFailure)
                    {
                        int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgAccNew, criteria.BarCondition, false, criteria.TotalTrial, data.FreDurMoveStat);
                        if (AccConsecutiveCount < criteria.FailureNeeded)
                            return false;
                    } //--- [New Criteria] May 2020 --|avgAcc|--(End)-- //
                    else if (criteria.ConsecutiveFailures)
                    {
                        int AccConsecutiveCount = ConsecutiveCountfreqd(avgAcc, criteria.BarCondition, false);
                        if (AccConsecutiveCount < criteria.FailureNeeded)
                            return false;
                    }
                    else
                    {
                        int AccCount = SuccessORFailureCountFrequencyDuration(avgAcc, criteria.BarCondition, criteria.TotalTrial, false);
                        //if (AccCount < criteria.FailureNeeded)
                        //    return false;
                        if (criteria.FailureNeeded < criteria.TotalTrial)
                        {
                            if (AccCount < (criteria.FailureNeeded + 1))
                                return false;
                        }
                        else
                        {
                            if (AccCount < criteria.FailureNeeded)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        public static Result Execute(InputData data, bool PrevStatus, bool bPromptColumn, string chaintype)
        {

            Result res = GetNextStepAndPrompt(data, bPromptColumn);
            res.CompletionStatus = "NOT COMPLETED";
            bool stillPromptExist = false;
            if (res.NextStep > data.CurrentStep)
                res.MovedForwardStep = true;
            if (PromptIndex(data.PromptsUsed, res.NextPrompt) > PromptIndex(data.PromptsUsed, data.CurrentPrompt))
                res.MovedForwardPrompt = true;
            if (!PrevStatus)
            {
                if (data.NoPromptsUsed.Length > 0)
                {
                    if (PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) == PromptIndex(data.NoPromptsUsed, data.NoPromptsUsed[data.NoPromptsUsed.Length - 1]))
                    {
                        res.MovedForwardPrompt = false;
                    }
                }
            }



            if (res.NextStep > data.StepCount)
            {
                res.MovedForwardStep = false;
                if (!bPromptColumn)
                {
                    res.MovedForwardStep = true;
                    if ((data.NoPromptsUsed.Length > 0) && (data.promptUp == 0))
                    {
                        if (PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) >= PromptIndex(data.NoPromptsUsed, data.NoPromptsUsed[data.NoPromptsUsed.Length - 1]))
                        {
                            res.MovedForwardStep = false;
                            stillPromptExist = true;
                        }
                        else
                            res.NextStep = data.CurrentStep;
                    }
                }
            }


            //if (res.nextstep == data.stepcount)
            //{
            //    if (data.nopromptsused.length > 0)
            //    {
            //        if (promptindex(data.nopromptsused, data.scurrentlessonprompt) < promptindex(data.nopromptsused, data.nopromptsused[data.nopromptsused.length - 1]))
            //        {
            //            res.movedforwardstep = true;
            //        }
            //    }
            //}

            int iStartIndex = PromptIndex(data.PromptsUsed, data.TargetPrompt);
            bool bSuccess = false;
            bool bsucs = true;
            int curr = data.next;
            int marked_count1 = data.marked;
            if (res.NextStep > data.StepCount)
            {
                res.NextStep = data.StepCount;
                res.MovedForwardStep = false;
                float[] avgCorrect = new float[data.Sessions.Length];
                float[] avgIncorrect = new float[data.Sessions.Length];
                float[] avgSetFre = new float[data.Sessions.Length];
                float[] avgSetAvgDur = new float[data.Sessions.Length];
                float[] avgSetTotDur = new float[data.Sessions.Length];

                float[] avgSetFreNew = new float[data.Sessions.Length];
                float[] avgSetAvgDurNew = new float[data.Sessions.Length];
                float[] avgSetTotDurNew = new float[data.Sessions.Length];
                for (int i = 0; i < data.TrialsData.Length; i++)
                {
                    avgCorrect[i] = data.TrialsData[i].Correct;
                    avgIncorrect[i] = data.TrialsData[i].Incorrect;
                }
                for (int i = iStartIndex; i < data.PromptsUsed.Length; i++)
                {
                    float[] avgAcc = new float[data.Sessions.Length];
                    float[] avgInd = new float[data.Sessions.Length];
                    float[] avgLearned = new float[data.Sessions.Length];
                    float[] avgExcludeCrnt = new float[data.Sessions.Length];
                    //float CrctRespCount = 0;
                    //int stepCount = 0;
                    float CrctRespCountat = 0; bSuccess = true;
                    for (int k = 0; k < data.Sessions.Length; k++)
                    {
                        float FreqScoreSum = 0;
                        double getTime = 0;
                        double AvgDurScoreSum = 0;
                        double TotDurScoreSum = 0;
                        int avgscorecount = 0;
                        if ((data.Sessions[k].Steps[data.CurrentStep - 1].Score != null) && (data.Sessions[k].Steps[data.CurrentStep - 1].Score.ToString() != ""))
                        {
                            if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[data.CurrentStep - 1].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                            {
                                avgAcc[k] = 1;
                            }
                            else
                            {
                                avgAcc[k] = 0;
                            }
                            if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[data.CurrentStep - 1].Score) == (data.PromptsUsed.Length - 1))
                            {
                                avgInd[k] = 1;
                            }
                            else
                            {
                                avgInd[k] = 0;
                            }
                            //train
                            if (data.PromptLearnedStepMoveUp.TotalTrial > 0 || data.LearnedStepMoveUp.TotalTrial > 0 || data.SetLearnedStepMoveUp.TotalTrial > 0)
                            {
                                if (data.next == data.StepCount)
                                {
                                    data.next = data.next - 1;
                                }
                                if (data.next == 0)
                                {
                                    data.next = 1;
                                }
                                if (data.PromptsUsed.Length > 0)
                                {
                                    if (data.Sessions[k].Steps[data.next - 1].Score == data.CorrectResp)
                                    {
                                        CrctRespCountat++;
                                    }
                                }
                                avgLearned[k] = CrctRespCountat;
                                CrctRespCountat = 0;
                            }

                            if (data.FrequencyMoveUp.TotalTrial > 0)
                            {
                                int GetStat = data.FreDurMoveStat;
                                string getSetScore = "";
                                for (int fr = 0; fr < data.Sessions[k].Steps.Length; fr++)
                                {
                                    FreqScoreSum = FreqScoreSum + float.Parse(data.Sessions[k].Steps[fr].Score);
                                }

                                if (GetStat == 1)
                                {
                                    if (Convert.ToInt32(FreqScoreSum) < data.FrequencyMoveUp.BarCondition)
                                    {
                                    getSetScore = "+";
                                }
                                else
                                {
                                    getSetScore = "-";
                                }
                                }
                                else if (GetStat == 0)
                                {
                                    if (Convert.ToInt32(FreqScoreSum) > data.FrequencyMoveUp.BarCondition)
                                    {
                                        getSetScore = "+";
                                    }
                                    else
                                    {
                                        getSetScore = "-";
                                    }
                                }
                                avgSetFreNew[k] = FreqScoreSum;
                                if (PromptIndex(data.PromptsUsed, getSetScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                {
                                    avgSetFre[k] = 1;
                        }
                                else
                                {
                                    avgSetFre[k] = 0;
                                }
                            }

                            if (data.AvgDurationMoveUp.TotalTrial > 0)
                            {
                                int GetStat = data.FreDurMoveStat;
                                string getSetScore = "";
                                for (int ad = 0; ad < data.Sessions[k].Steps.Length; ad++)
                                {                                    
                                    AvgDurScoreSum = AvgDurScoreSum + TimeSpan.Parse(data.Sessions[k].Steps[ad].Score).TotalSeconds;
                                    avgscorecount++;
                                }
                                
                                if (avgscorecount > 0)
                                {
                                    AvgDurScoreSum = AvgDurScoreSum / avgscorecount;
                                }
                                if (GetStat == 1)
                                {
                                    if (Convert.ToInt32(AvgDurScoreSum) < data.AvgDurationMoveUp.BarCondition)
                                    {
                                        getSetScore = "+";
                                    }
                        else
                        {
                                        getSetScore = "-";
                                    }
                                }
                                else if (GetStat == 0)
                                {
                                    if (Convert.ToInt32(AvgDurScoreSum) > data.AvgDurationMoveUp.BarCondition)
                                    {
                                        getSetScore = "+";
                                    }
                                    else
                                    {
                                        getSetScore = "-";
                                    }
                                }
                                float totdur = (float)AvgDurScoreSum;
                                avgSetAvgDurNew[k] = totdur;
                                
                                if (PromptIndex(data.PromptsUsed, getSetScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                {
                                    avgSetAvgDur[k] = 1;
                                }
                                else
                                {
                                    avgSetAvgDur[k] = 0;
                                }
                            }

                            if (data.TotalDurationMoveUp.TotalTrial > 0)
                            {
                                int GetStat = data.FreDurMoveStat;
                                string getSetScore = "";
                                for (int td = 0; td < data.Sessions[k].Steps.Length; td++)
                                {
                                    TotDurScoreSum = TotDurScoreSum + TimeSpan.Parse(data.Sessions[k].Steps[td].Score).TotalSeconds;
                                }   

                                if (GetStat == 1)
                                {
                                    if (Convert.ToInt32(TotDurScoreSum) < data.TotalDurationMoveUp.BarCondition)
                                    {
                                        getSetScore = "+";
                                    }
                                    else
                                    {
                                        getSetScore = "-";
                                    }
                                }
                                else if (GetStat == 0)
                                {
                                    if (Convert.ToInt32(TotDurScoreSum) > data.TotalDurationMoveUp.BarCondition)
                                    {
                                        getSetScore = "+";
                                    }
                                    else
                                    {
                                        getSetScore = "-";
                                    }
                                }

                                float avgdur = (float)TotDurScoreSum;
                                avgSetTotDurNew[k] = avgdur;

                                if (PromptIndex(data.PromptsUsed, getSetScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                {
                                    avgSetTotDur[k] = 1;
                                }
                                else
                                {
                                    avgSetTotDur[k] = 0;
                                }
                            } 
                        }
                        else
                        {
                            avgAcc[k] = -9999;
                            avgInd[k] = -9999;
                            //avgLearned[k] = 0;
                            //avgExcludeCrnt[k] = -9999;


                        }

                        if (data.PromptExcludeCrntStepMoveUp.TotalTrial > 0 || data.ExcludeCrntStepMoveUp.TotalTrial > 0 || data.SetExcludeCrntStepMoveUp.TotalTrial > 0)
                        {
                            if (data.next == data.StepCount)
                            {
                                data.next = data.next - 1;
                            }
                            if (data.next == 0)
                            {
                                data.next = 1;
                            }
                            if ((data.Sessions[k].Steps[data.CurrentStep - 1].Score != null) && (data.Sessions[k].Steps[data.CurrentStep - 1].Score.ToString() != ""))
                            {
                                if (data.PromptsUsed.Length > 0)
                                {
                                    if (avgCorrect[k] + avgIncorrect[k] == 1 || avgCorrect[k] + avgIncorrect[k] == 0)
                                    {
                                        if ((data.Sessions[k].Steps[data.next - 1].Score != null) && (data.Sessions[k].Steps[data.next - 1].Score.ToString() != ""))
                                        {
                                            avgExcludeCrnt[k] = 0;
                                        }
                                        else
                                        {

                                            if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[data.CurrentStep - 1].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                            {
                                                avgExcludeCrnt[k] = 1;
                                            }

                                            else
                                            {
                                                avgExcludeCrnt[k] = 0;
                                                //bSuccess = false;
                                            }

                                        }

                                    }
                                    else if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[data.CurrentStep - 1].Score) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                    {
                                        avgExcludeCrnt[k] = 1;
                                    }

                                    else
                                    {
                                        if (data.CurrentStep - 1 == data.next - 1)
                                        {
                                            avgExcludeCrnt[k] = 1;
                                        }
                                        else
                                        {
                                            avgExcludeCrnt[k] = 0;
                                            //bSuccess = false;
                                        }
                                    }


                                }
                            }
                            else
                            {
                                if (data.PromptsUsed.Length > 0)
                                {

                                    avgExcludeCrnt[k] = -9999;
                                }


                            }
                        }


                        //avgAcc[k] = data.Sessions[k].Steps[data.Sessions[k].Steps.Length - 1].PromptAverages[data.PromptsUsed[i]];
                        //avgInd[k] = data.Sessions[k].Steps[data.Sessions[k].Steps.Length - 1].PromptAverages[data.PromptsUsed[data.PromptsUsed.Length - 1]];
                    }

                    if (data.FrequencyMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|FrequencyMoveUp|--(Start)-- //
                        if (data.FrequencyMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetFreNew, data.FrequencyMoveUp.BarCondition, true, data.FrequencyMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.FrequencyMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|FrequencyMoveUp|--(End)-- //
                        else if (data.FrequencyMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetFre, data.FrequencyMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.FrequencyMoveUp.SuccessNeeded)
                                bSuccess = false;
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgSetFre, data.FrequencyMoveUp.BarCondition, data.FrequencyMoveUp.TotalTrial, true);
                            if (AccCount < data.FrequencyMoveUp.SuccessNeeded)
                                bSuccess = false;
                        }
                    }

                    if (data.AvgDurationMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|AvgDurationMoveUp|--(Start)-- //
                        if (data.AvgDurationMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetAvgDurNew, data.AvgDurationMoveUp.BarCondition, true, data.AvgDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.AvgDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|AvgDurationMoveUp|--(End)-- //
                        else if (data.AvgDurationMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetAvgDur, data.AvgDurationMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.AvgDurationMoveUp.SuccessNeeded)
                                bSuccess = false;
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgSetAvgDur, data.AvgDurationMoveUp.BarCondition, data.AvgDurationMoveUp.TotalTrial, true);
                            if (AccCount < data.AvgDurationMoveUp.SuccessNeeded)
                                bSuccess = false;
                        }
                    }

                    if (data.TotalDurationMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|TotalDurationMoveUp|--(Start)-- //
                        if (data.TotalDurationMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetTotDurNew, data.TotalDurationMoveUp.BarCondition, true, data.TotalDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.TotalDurationMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|TotalDurationMoveUp|--(End)-- //
                        else if (data.TotalDurationMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetTotDur, data.TotalDurationMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.TotalDurationMoveUp.SuccessNeeded)
                                bSuccess = false;
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgSetTotDur, data.TotalDurationMoveUp.BarCondition, data.TotalDurationMoveUp.TotalTrial, true);
                            if (AccCount < data.TotalDurationMoveUp.SuccessNeeded)
                                bSuccess = false;
                        }
                    } 

                    if (data.PercentAccuracy.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|PercentAccuracy|--(Start)-- //
                        if (data.PercentAccuracy.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgAcc, data.PercentAccuracy.BarCondition, true, data.PercentAccuracy.TotalTrial);
                            if (AccConsecutiveCount < data.PercentAccuracy.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                        else if (data.PercentAccuracy.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgAcc, data.PercentAccuracy.BarCondition, true);
                            if (AccConsecutiveCount < data.PercentAccuracy.SuccessNeeded)
                                bSuccess = false;
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgAcc, data.PercentAccuracy.BarCondition, data.PercentAccuracy.TotalTrial, true);
                            if (AccCount < data.PercentAccuracy.SuccessNeeded)
                                bSuccess = false;
                        }
                    }
                    if (data.PercentIndependence.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|PercentIndependence|--(Start)-- //
                        if (data.PercentIndependence.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgAcc, data.PercentIndependence.BarCondition, true, data.PercentIndependence.TotalTrial);
                            if (AccConsecutiveCount < data.PercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|PercentIndependence|--(End)-- //
                        else if (data.PercentIndependence.ConsecutiveSuccess)
                        {
                            int IndConsecutiveCount = ConsecutiveCount(avgInd, data.PercentIndependence.BarCondition, true);
                            if (IndConsecutiveCount < data.PercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            int IndCount = SuccessORFailureCount(avgInd, data.PercentIndependence.BarCondition, data.PercentIndependence.TotalTrial, true);
                            if (IndCount < data.PercentIndependence.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }
                    }
                    if (data.SetTotalCorrectMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|SetTotalCorrectMoveUp|--(Start)-- //
                        if (data.SetTotalCorrectMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, true, data.SetTotalCorrectMoveUp.TotalTrial);
                            if (AccConsecutiveCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|SetTotalCorrectMoveUp|--(End)-- //
                        else if (data.SetTotalCorrectMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                                bSuccess = false;
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, data.SetTotalCorrectMoveUp.TotalTrial, true);
                            if (AccCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                                bSuccess = false;
                        }
                    }
                    if (data.SetLearnedStepMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|SetLearnedStepMoveUp|--(Start)-- //
                        if (data.SetLearnedStepMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgAcc, data.SetLearnedStepMoveUp.BarCondition, true, data.SetLearnedStepMoveUp.TotalTrial);
                            if (AccConsecutiveCount < data.SetLearnedStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|SetLearnedStepMoveUp|--(End)-- //
                        else if (data.SetLearnedStepMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgLearned, data.SetLearnedStepMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.SetLearnedStepMoveUp.SuccessNeeded)
                                bSuccess = false;
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgLearned, data.SetLearnedStepMoveUp.BarCondition, data.SetLearnedStepMoveUp.TotalTrial, true);
                            if (AccCount < data.SetLearnedStepMoveUp.SuccessNeeded)
                                bSuccess = false;
                        }
                    }

                    if (data.SetExcludeCrntStepMoveUp.TotalTrial > 0)
                    {
                        //--- [New Criteria] May 2020 --|SetExcludeCrntStepMoveUp|--(Start)-- //
                        if (data.SetExcludeCrntStepMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgAcc, data.SetExcludeCrntStepMoveUp.BarCondition, true, data.SetExcludeCrntStepMoveUp.TotalTrial);
                            if (AccConsecutiveCount < data.SetExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|SetExcludeCrntStepMoveUp|--(End)-- //
                        else if (data.SetExcludeCrntStepMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgExcludeCrnt, data.SetExcludeCrntStepMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.SetExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                                bsucs = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgExcludeCrnt, data.SetExcludeCrntStepMoveUp.BarCondition, data.SetExcludeCrntStepMoveUp.TotalTrial, true);
                            if (AccCount < data.SetExcludeCrntStepMoveUp.SuccessNeeded)
                            {
                                bSuccess = false;
                                bsucs = false;
                            }
                        }
                    }

                    if (bSuccess)
                        break;
                }
            }

            if (bSuccess)
            {

                if (data.CurrentSet == data.TotalSets)
                {
                    res.CompletionStatus = "COMPLETED";
                    res.NextSet = data.CurrentSet;
                    // res.NextStep = data.TotalSets;
                }
                else
                {
                    data.CurrentSet++;
                    res.MovedForwardSet = true;
                    res.NextStep = 1;
                    res.NextSet = data.CurrentSet;
                    //res.NextPrompt = data.PromptsUsed[0];
                    res.MovedForwardStep = false;
                    res.MovedForwardPrompt = false;
                }
            }





            if (res.MovedForwardPrompt || res.MovedForwardStep || res.MovedForwardSet)
                return res;

            //Check Move Back Criterias
            if (bPromptColumn)// Check if the colum is prompt.
            {
                if (!res.MovedForwardPrompt && data.CurrentPrompt != data.PromptsUsed[0])
                {
                    if (data.PromptFrequencyMoveDown.TotalTrial > 0 && CheckMoveBackFreDur(data, data.PromptFrequencyMoveDown, data.CurrentPrompt, false))
                    {
                        res.MovedBackPrompt = true;
                        res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, data.CurrentPrompt) - 1];
                        if (!PrevStatus)
                        {
                            res.NextStep = data.CurrentStep;
                            res.NextSet = data.CurrentSet;
                        }
                    }

                    if (data.PromptAvgDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.PromptAvgDurationMoveDown, data.CurrentPrompt, false))
                    {
                        res.MovedBackPrompt = true;
                        res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, data.CurrentPrompt) - 1];
                        if (!PrevStatus)
                        {
                            res.NextStep = data.CurrentStep;
                            res.NextSet = data.CurrentSet;
                        }
                    }

                    if (data.PromptTotalDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.PromptTotalDurationMoveDown, data.CurrentPrompt, false))
                    {
                        res.MovedBackPrompt = true;
                        res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, data.CurrentPrompt) - 1];
                        if (!PrevStatus)
                        {
                            res.NextStep = data.CurrentStep;
                            res.NextSet = data.CurrentSet;
                        }
                    }

                    //Check for Move back Prompt
                    //Check the prompt criterias for current step and current prompt and above prompts
                    if (data.MoveBackPromptPercentAccuracy.TotalTrial > 0 && CheckMoveBack(data, data.MoveBackPromptPercentAccuracy, data.CurrentPrompt, false))
                    {
                        res.MovedBackPrompt = true;
                        res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, data.CurrentPrompt) - 1];
                        if (!PrevStatus)
                        {
                            res.NextStep = data.CurrentStep;
                            res.NextSet = data.CurrentSet;
                        }


                    }
                    if (data.PromptTotalIncorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.PromptTotalIncorrectMoveBack, data.CurrentPrompt, false, false))
                    {
                        res.MovedBackPrompt = true;
                        res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, data.CurrentPrompt) - 1];
                        if (!PrevStatus)
                        {
                            res.NextStep = data.CurrentStep;
                            res.NextSet = data.CurrentSet;
                        }


                    }
                    if (data.PromptTotalCorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.PromptTotalCorrectMoveBack, data.CurrentPrompt, false, true))
                    {
                        res.MovedBackPrompt = true;
                        res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, data.CurrentPrompt) - 1];
                        if (!PrevStatus)
                        {
                            res.NextStep = data.CurrentStep;
                            res.NextSet = data.CurrentSet;
                        }


                    }
                    if (data.PromptLearnedStepMoveBack.TotalTrial > 0 && CheckMoveBack1(data, data.PromptLearnedStepMoveBack, data.CurrentPrompt, false))
                    {
                        res.MovedBackPrompt = true;
                        res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, data.CurrentPrompt) - 1];
                        if (!PrevStatus)
                        {
                            res.NextStep = data.CurrentStep;
                            res.NextSet = data.CurrentSet;
                        }


                    }
                    if (data.PromptExcludeCrntStepMoveBack.TotalTrial > 0 && CheckMoveBack2(data, data.PromptExcludeCrntStepMoveBack, data.CurrentPrompt, false))
                    {
                        res.MovedBackPrompt = true;
                        res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, data.CurrentPrompt) - 1];
                        if (!PrevStatus)
                        {
                            res.NextStep = data.CurrentStep;
                            res.NextSet = data.CurrentSet;
                        }


                    }

                    if (data.MoveBackPromptPercentAllIndependence.TotalTrial > 0 && CheckMoveBackPer(data, data.MoveBackPromptPercentAllIndependence, data.CurrentPrompt, false))
                    {
                        res.MovedBackPrompt = true;
                        res.NextPrompt = data.PromptsUsed[PromptIndex(data.PromptsUsed, data.CurrentPrompt) - 1];
                        if (!PrevStatus)
                        {
                            res.NextStep = data.CurrentStep;
                            res.NextSet = data.CurrentSet;
                        }


                    }
                }
            }
            else
            {
                if (data.NoPromptsUsed.Length > 0)
                {
                    if (!res.MovedForwardPrompt && data.sCurrentLessonPrompt != data.NoPromptsUsed[0])
                    {

                        if (data.PromptFrequencyMoveDown.TotalTrial > 0 && CheckMoveBackFreDur(data, data.PromptFrequencyMoveDown, data.CurrentPrompt, false))
                        {
                            res.MovedBackPrompt = true;
                            res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) - 1];
                            if (!PrevStatus)
                            {
                                res.NextStep = data.CurrentStep;
                                res.NextSet = data.CurrentSet;
                            }
                        }

                        if (data.PromptAvgDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.PromptAvgDurationMoveDown, data.CurrentPrompt, false))
                        {
                            res.MovedBackPrompt = true;
                            res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) - 1];
                            if (!PrevStatus)
                            {
                                res.NextStep = data.CurrentStep;
                                res.NextSet = data.CurrentSet;
                            }
                        }

                        if (data.PromptTotalDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.PromptTotalDurationMoveDown, data.CurrentPrompt, false))
                        {
                            res.MovedBackPrompt = true;
                            res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) - 1];
                            if (!PrevStatus)
                            {
                                res.NextStep = data.CurrentStep;
                                res.NextSet = data.CurrentSet;
                            }
                        }

                        //Check for Move back Prompt
                        //Check the prompt criterias for current step and current prompt and above prompts
                        if (data.MoveBackPromptPercentAccuracy.TotalTrial > 0 && CheckMoveBack(data, data.MoveBackPromptPercentAccuracy, data.CurrentPrompt, false))
                        {
                            res.MovedBackPrompt = true;
                            res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) - 1];
                            if (!PrevStatus)
                            {
                                res.NextStep = data.CurrentStep;
                                res.NextSet = data.CurrentSet;
                            }


                        }
                        if (data.MoveBackPromptPercentIndependence.TotalTrial > 0 && CheckMoveBack(data, data.MoveBackPromptPercentIndependence, data.CurrentPrompt, false))
                        {
                            res.MovedBackPrompt = true;
                            res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) - 1];
                            if (!PrevStatus)
                            {
                                res.NextStep = data.CurrentStep;
                                res.NextSet = data.CurrentSet;
                            }
                        }
                        if (data.PromptTotalIncorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.PromptTotalIncorrectMoveBack, data.CurrentPrompt, false, false))
                        {
                            res.MovedBackPrompt = true;
                            res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) - 1];
                            if (!PrevStatus)
                            {
                                res.NextStep = data.CurrentStep;
                                res.NextSet = data.CurrentSet;
                            }


                        }
                        if (data.PromptTotalCorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.PromptTotalCorrectMoveBack, data.CurrentPrompt, false, true))
                        {
                            res.MovedBackPrompt = true;
                            res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) - 1];
                            if (!PrevStatus)
                            {
                                res.NextStep = data.CurrentStep;
                                res.NextSet = data.CurrentSet;
                            }


                        }
                        if (data.PromptLearnedStepMoveBack.TotalTrial > 0 && CheckMoveBack1(data, data.PromptLearnedStepMoveBack, data.CurrentPrompt, false))
                        {
                            res.MovedBackPrompt = true;
                            res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) - 1];
                            if (!PrevStatus)
                            {
                                res.NextStep = data.CurrentStep;
                                res.NextSet = data.CurrentSet;
                            }


                        }

                        if (data.PromptExcludeCrntStepMoveBack.TotalTrial > 0 && CheckMoveBack2(data, data.PromptExcludeCrntStepMoveBack, data.CurrentPrompt, false))
                        {
                            res.MovedBackPrompt = true;
                            res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) - 1];
                            if (!PrevStatus)
                            {
                                res.NextStep = data.CurrentStep;
                                res.NextSet = data.CurrentSet;
                            }


                        }

                        if (data.MoveBackPromptPercentAllIndependence.TotalTrial > 0 && CheckMoveBackPer(data, data.MoveBackPromptPercentAllIndependence, data.CurrentPrompt, false))
                        {
                            res.MovedBackPrompt = true;
                            res.NextPrompt = data.NoPromptsUsed[PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) - 1];
                            if (!PrevStatus)
                            {
                                res.NextStep = data.CurrentStep;
                                res.NextSet = data.CurrentSet;
                            }


                        }
                    }
                }
            }
            if (!bPromptColumn)// Check if the colum is prompt.
            {
                if (data.NoPromptsUsed.Length > 0)
                {
                    if (PromptIndex(data.NoPromptsUsed, res.NextPrompt) > PromptIndex(data.NoPromptsUsed, data.NoPromptsUsed[0]))
                    {
                        return res;
                    }
                }
            }
            if (data.CurrentStep >= 1 && !res.MovedForwardStep && !res.MovedBackPrompt)
            {
                bool bMoveback = false;
                //Check for Move back Step 
                //Check the step criterias for current step and current prompt and above prompts

                //Code changed for avoid step move back for current step if the criteria meet even in prompt move up
                if (PrevStatus)// data.CurrentPrompt == data.PromptsUsed[0])
                {
                    if (bPromptColumn)// Check if the colum is prompt.
                    {
                        if (!res.MovedForwardStep && data.CurrentPrompt == data.PromptsUsed[0])
                        {
                            bMoveback = false;

                            if (data.StepFrequencyMoveDown.TotalTrial > 0 && CheckMoveBackFreDur(data, data.StepFrequencyMoveDown, data.CurrentPrompt, false))
                            {
                                bMoveback = true;
                            }

                            if (data.StepAvgDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepAvgDurationMoveDown, data.CurrentPrompt, false))
                            {
                                bMoveback = true;
                            }

                            if (data.StepTotalDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepTotalDurationMoveDown, data.CurrentPrompt, false))
                            {
                                bMoveback = true;
                            }

                            if (data.StepMoveBackPercentAccuracy.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentAccuracy, data.CurrentPrompt, false))
                            {
                                bMoveback = true;

                            }
                            if (data.StepMoveBackPercentIndependence.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentIndependence, data.CurrentPrompt, true))
                            {
                                bMoveback = true;
                            }
                            if (data.StepTotalIncorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalIncorrectMoveBack, data.CurrentPrompt, false, false))
                            {
                                bMoveback = true;

                            }
                            if (data.StepTotalCorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalCorrectMoveBack, data.CurrentPrompt, false, true))
                            {
                                bMoveback = true;

                            }
                            if (data.LearnedStepMoveBack.TotalTrial > 0 && CheckMoveBack1(data, data.LearnedStepMoveBack, data.CurrentPrompt, false))
                            {
                                bMoveback = true;

                            }
                            if (data.ExcludeCrntStepMoveBack.TotalTrial > 0 && CheckMoveBack2(data, data.ExcludeCrntStepMoveBack, data.CurrentPrompt, false))
                            {
                                bMoveback = true;

                            }
                            if (data.StepMoveBackPercentAllIndependence.TotalTrial > 0 && CheckMoveBackPer(data, data.StepMoveBackPercentAllIndependence, data.CurrentPrompt, false))
                            {
                                bMoveback = true;
                            }
                        }
                    }
                    else
                    {
                        if (data.NoPromptsUsed.Length > 0 && (data.promptDown >= 1))
                        {
                            if (data.promptType == "Least-to-Most" || data.promptType == "Graduated Guidance")
                            {
                                if (!res.MovedForwardStep && data.sCurrentLessonPrompt == data.NoPromptsUsed[data.NoPromptsUsed.Length - 1])
                                {

                                    bMoveback = false;

                                    if (data.StepFrequencyMoveDown.TotalTrial > 0 && CheckMoveBackFreDur(data, data.StepFrequencyMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepAvgDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepAvgDurationMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepTotalDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepTotalDurationMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepMoveBackPercentAccuracy.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentAccuracy, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepMoveBackPercentIndependence.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentIndependence, data.CurrentPrompt, true))
                                    {
                                        bMoveback = true;
                                    }
                                    if (data.StepTotalIncorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalIncorrectMoveBack, data.CurrentPrompt, false, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepTotalCorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalCorrectMoveBack, data.CurrentPrompt, false, true))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.LearnedStepMoveBack.TotalTrial > 0 && CheckMoveBack1(data, data.LearnedStepMoveBack, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }

                                    if (data.ExcludeCrntStepMoveBack.TotalTrial > 0 && CheckMoveBack2(data, data.ExcludeCrntStepMoveBack, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepMoveBackPercentAllIndependence.TotalTrial > 0 && CheckMoveBackPer(data, data.StepMoveBackPercentAllIndependence, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                }
                            }
                            else
                            {
                                if (!res.MovedForwardStep && data.sCurrentLessonPrompt == data.NoPromptsUsed[0])
                                {

                                    bMoveback = false;

                                    if (data.StepFrequencyMoveDown.TotalTrial > 0 && CheckMoveBackFreDur(data, data.StepFrequencyMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepAvgDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepAvgDurationMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepTotalDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepTotalDurationMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepMoveBackPercentAccuracy.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentAccuracy, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepMoveBackPercentIndependence.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentIndependence, data.CurrentPrompt, true))
                                    {
                                        bMoveback = true;
                                    }
                                    if (data.StepTotalIncorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalIncorrectMoveBack, data.CurrentPrompt, false, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepTotalCorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalCorrectMoveBack, data.CurrentPrompt, false, true))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.LearnedStepMoveBack.TotalTrial > 0 && CheckMoveBack1(data, data.LearnedStepMoveBack, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }

                                    if (data.ExcludeCrntStepMoveBack.TotalTrial > 0 && CheckMoveBack2(data, data.ExcludeCrntStepMoveBack, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepMoveBackPercentAllIndependence.TotalTrial > 0 && CheckMoveBackPer(data, data.StepMoveBackPercentAllIndependence, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }
                                }
                            }
                        }

                    }

                }

                if (bPromptColumn)// Check if the colum is prompt.
                {
                    //
                    if (data.StepMoveBackPercentAccuracy.TotalTrial > 0 || data.StepMoveBackPercentIndependence.TotalTrial > 0 || data.StepTotalIncorrectMoveBack.TotalTrial > 0 || data.StepTotalCorrectMoveBack.TotalTrial > 0 || data.StepMoveBackPercentAllIndependence.TotalTrial > 0 || data.StepFrequencyMoveDown.TotalTrial > 0 || data.StepAvgDurationMoveDown.TotalTrial > 0 || data.StepTotalCorrectMoveBack.TotalTrial > 0)
                    {
                        if (!res.MovedForwardStep && data.CurrentPrompt == data.PromptsUsed[0])
                        {

                            bMoveback = false;

                            if (data.StepFrequencyMoveDown.TotalTrial > 0 && CheckMoveBackFreDur(data, data.StepFrequencyMoveDown, data.CurrentPrompt, false))
                            {
                                bMoveback = true;
                            }

                            if (data.StepAvgDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepAvgDurationMoveDown, data.CurrentPrompt, false))
                            {
                                bMoveback = true;
                            }

                            if (data.StepTotalDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepTotalDurationMoveDown, data.CurrentPrompt, false))
                            {
                                bMoveback = true;
                            }

                            if (data.StepMoveBackPercentAccuracy.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentAccuracy, data.CurrentPrompt, false))
                            {
                                bMoveback = true;

                            }
                            if (data.StepMoveBackPercentIndependence.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentIndependence, data.CurrentPrompt, true))
                            {
                                bMoveback = true;
                            }
                            if (data.StepTotalIncorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalIncorrectMoveBack, data.CurrentPrompt, false, false))
                            {
                                bMoveback = true;

                            }
                            if (data.StepTotalCorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalCorrectMoveBack, data.CurrentPrompt, false, true))
                            {
                                bMoveback = true;

                            }
                            if (data.LearnedStepMoveBack.TotalTrial > 0 && CheckMoveBack1(data, data.LearnedStepMoveBack, data.CurrentPrompt, false))
                            {
                                bMoveback = true;

                            }

                            if (data.ExcludeCrntStepMoveBack.TotalTrial > 0 && CheckMoveBack2(data, data.ExcludeCrntStepMoveBack, data.CurrentPrompt, false))
                            {
                                bMoveback = true;

                            }
                            if (data.StepMoveBackPercentAllIndependence.TotalTrial > 0 && CheckMoveBackPer(data, data.StepMoveBackPercentAllIndependence, data.CurrentPrompt, false))
                            {
                                bMoveback = true;
                            }
                        }
                    }
                }
                else
                {
                    // 
                    if (data.StepMoveBackPercentAccuracy.TotalTrial > 0 || data.StepMoveBackPercentIndependence.TotalTrial > 0 || data.ExcludeCrntStepMoveBack.TotalTrial > 0 || data.SetLearnedStepMoveBack.TotalTrial > 0 || data.StepPercentAllIndependence.TotalTrial > 0 || data.StepFrequencyMoveDown.TotalTrial > 0 || data.StepAvgDurationMoveDown.TotalTrial > 0 || data.StepTotalDurationMoveDown.TotalTrial > 0 || data.StepTotalDurationMoveDown.TotalTrial > 0 || data.StepTotalCorrectMoveBack.TotalTrial >0 || data.StepTotalIncorrectMoveBack.TotalTrial >0)
                    {
                        if (data.NoPromptsUsed.Length > 0 && (data.promptDown >= 1))
                        {
                            if (data.promptType == "Least-to-Most" || data.promptType == "Graduated Guidance")
                            {
                                if (!res.MovedForwardStep && data.sCurrentLessonPrompt == data.NoPromptsUsed[data.NoPromptsUsed.Length - 1])
                                {

                                    bMoveback = false;

                                    if (data.StepFrequencyMoveDown.TotalTrial > 0 && CheckMoveBackFreDur(data, data.StepFrequencyMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepAvgDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepAvgDurationMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepTotalDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepTotalDurationMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepMoveBackPercentAccuracy.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentAccuracy, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepMoveBackPercentIndependence.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentIndependence, data.CurrentPrompt, true))
                                    {
                                        bMoveback = true;
                                    }
                                    if (data.StepTotalIncorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalIncorrectMoveBack, data.CurrentPrompt, false, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepTotalCorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalCorrectMoveBack, data.CurrentPrompt, false, true))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.LearnedStepMoveBack.TotalTrial > 0 && CheckMoveBack1(data, data.LearnedStepMoveBack, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }

                                    if (data.ExcludeCrntStepMoveBack.TotalTrial > 0 && CheckMoveBack2(data, data.ExcludeCrntStepMoveBack, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepMoveBackPercentAllIndependence.TotalTrial > 0 && CheckMoveBackPer(data, data.StepMoveBackPercentAllIndependence, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }
                                }
                            }
                            else
                            {
                                if (!res.MovedForwardStep && data.sCurrentLessonPrompt == data.NoPromptsUsed[0])
                                {

                                    bMoveback = false;

                                    if (data.StepFrequencyMoveDown.TotalTrial > 0 && CheckMoveBackFreDur(data, data.StepFrequencyMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepAvgDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepAvgDurationMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepTotalDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepTotalDurationMoveDown, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }

                                    if (data.StepMoveBackPercentAccuracy.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentAccuracy, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepMoveBackPercentIndependence.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentIndependence, data.CurrentPrompt, true))
                                    {
                                        bMoveback = true;
                                    }
                                    if (data.StepTotalIncorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalIncorrectMoveBack, data.CurrentPrompt, false, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepTotalCorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalCorrectMoveBack, data.CurrentPrompt, false, true))
                                    {
                                        bMoveback = true;

                                    }
                                    //if ((data.StepMoveBackPercentAccuracy.TotalTrial > 0 || data.StepMoveBackPercentIndependence.TotalTrial > 0) && (data.promptDown == 1))
                                    //{
                                    //    bMoveback = true;
                                    //}
                                    if (data.LearnedStepMoveBack.TotalTrial > 0 && CheckMoveBack1(data, data.LearnedStepMoveBack, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }


                                    if (data.ExcludeCrntStepMoveBack.TotalTrial > 0 && CheckMoveBack2(data, data.ExcludeCrntStepMoveBack, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;

                                    }
                                    if (data.StepMoveBackPercentAllIndependence.TotalTrial > 0 && CheckMoveBackPer(data, data.StepMoveBackPercentAllIndependence, data.CurrentPrompt, false))
                                    {
                                        bMoveback = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!res.MovedForwardStep && data.CurrentPrompt == data.PromptsUsed[0]) //Step Movebak  if prompt not used
                            {

                                bMoveback = false;

                                if (data.StepFrequencyMoveDown.TotalTrial > 0 && CheckMoveBackFreDur(data, data.StepFrequencyMoveDown, data.CurrentPrompt, false))
                                {
                                    bMoveback = true;
                                }

                                if (data.StepAvgDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepAvgDurationMoveDown, data.CurrentPrompt, false))
                                {
                                    bMoveback = true;
                                }

                                if (data.StepTotalDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.StepTotalDurationMoveDown, data.CurrentPrompt, false))
                                {
                                    bMoveback = true;
                                }

                                if (data.StepMoveBackPercentAccuracy.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentAccuracy, data.CurrentPrompt, false))
                                {
                                    bMoveback = true;

                                }
                                if (data.StepMoveBackPercentIndependence.TotalTrial > 0 && CheckMoveBack(data, data.StepMoveBackPercentIndependence, data.CurrentPrompt, true))
                                {
                                    bMoveback = true;
                                }
                                if (data.StepTotalIncorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalIncorrectMoveBack, data.CurrentPrompt, false, false))
                                {
                                    bMoveback = true;

                                }
                                if (data.StepTotalCorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.StepTotalCorrectMoveBack, data.CurrentPrompt, false, true))
                                {
                                    bMoveback = true;

                                }
                                if (data.LearnedStepMoveBack.TotalTrial > 0 && CheckMoveBack1(data, data.LearnedStepMoveBack, data.CurrentPrompt, false))
                                {
                                    bMoveback = true;

                                }

                                if (data.ExcludeCrntStepMoveBack.TotalTrial > 0 && CheckMoveBack2(data, data.ExcludeCrntStepMoveBack, data.CurrentPrompt, false))
                                {
                                    bMoveback = true;

                                }
                                if (data.StepMoveBackPercentAllIndependence.TotalTrial > 0 && CheckMoveBackPer(data, data.StepMoveBackPercentAllIndependence, data.CurrentPrompt, false))
                                {
                                    bMoveback = true;
                                }
                            }
                        }

                    }
                }
                //Step Move BAck when data.CurrentPrompt == data.PromptsUsed[0]




                //if (data.LearnedStepMoveBack.TotalTrial > 0)
                //{
                //float[] avgLearned = new float[data.Sessions.Length];

                // for (int k = 0; k < data.Sessions.Length; k++)
                //{
                //  if (data.Sessions[k].Steps[data.CurrentStep - 1] != null)
                //{
                //  if (data.Sessions[k].Steps[data.CurrentStep - 1].Score == data.CorrectResp)
                //{
                //  avgLearned[k] = 1;
                // }
                //else
                //{
                //   avgLearned[k] = 0;
                // }
                //}
                //  }
                // if (data.LearnedStepMoveBack.ConsecutiveFailures)
                //{
                //     int AccConsecutiveCount = ConsecutiveCount(avgLearned, data.LearnedStepMoveBack.BarCondition, false);
                //     if (AccConsecutiveCount >= data.LearnedStepMoveBack.FailureNeeded)
                //       bMoveback = true;
                // }
                //else
                //{
                //   int AccCount = SuccessORFailureCount(avgLearned, data.LearnedStepMoveBack.BarCondition, data.LearnedStepMoveBack.TotalTrial, false);
                //   if (AccCount >= data.LearnedStepMoveBack.FailureNeeded)
                //       bMoveback = true;
                // }


                //  }




                //if (data.ExcludeCrntStepMoveBack.BarCondition > 0)
                //{
                //    float[] avgExcludeCrnt = new float[data.Sessions.Length];
                //    float CrctRespCount = 0;

                //    int stepCount = 0;
                //    for (int k = 0; k < data.Sessions.Length; k++)
                //    {
                //        for (int index = 0; index < data.Sessions[k].Steps.Length; index++)
                //        {
                //            if (data.Sessions[k].Steps[index] != data.Sessions[k].Steps[data.CurrentStep - 1])
                //            {
                //                if (data.Sessions[k].Steps[index].Score == data.CorrectResp)
                //                {
                //                    CrctRespCount++;
                //                }
                //                stepCount++;
                //            }
                //            else break;
                //        }
                //        avgExcludeCrnt[k] = CrctRespCount / stepCount;
                //        CrctRespCount = 0; stepCount = 0;
                //    }
                //    if (data.ExcludeCrntStepMoveBack.ConsecutiveFailures)
                //    {
                //        int AccConsecutiveCount = ConsecutiveCount(avgExcludeCrnt, data.ExcludeCrntStepMoveBack.BarCondition, false);
                //        if (AccConsecutiveCount >= data.ExcludeCrntStepMoveBack.FailureNeeded)
                //            bMoveback = true;
                //    }
                //    else
                //    {
                //        int AccCount = SuccessORFailureCount(avgExcludeCrnt, data.ExcludeCrntStepMoveBack.BarCondition, false);
                //        if (AccCount >= data.ExcludeCrntStepMoveBack.FailureNeeded)
                //            bMoveback = true;
                //    }


                //}
                if (bMoveback)
                {
                    res.MovedBackStep = true;
                    if (!PrevStatus)
                    {
                        if (data.CurrentStep == 1)
                        {
                            data.CurrentStep = 1;
                            res.MovedBackStep = false;
                        }
                        else
                            res.NextStep = data.CurrentStep - 1;
                    }
                    else
                        res.NextStep = data.CurrentStep - 1;
                    res.NextSet = data.CurrentSet;
                    res.MovedForwardPrompt = false;
                    res.NextPrompt = data.TargetPrompt;
                }


            }
            if (data.PromptsUsed.Length > 0)
            {
                bool prompt = false;
                if (data.promptType == "Least-to-Most" || data.promptType == "Graduated Guidance")
                {
                    if (data.CurrentPrompt == data.PromptsUsed[data.PromptsUsed.Length - 1])
                        prompt = true;
                }
                if (data.promptType == "Least-to-Most" || data.promptType == "Graduated Guidance" && data.MoveBackPercentAllIndependence.TotalTrial > 0)
                {
                    if (data.CurrentPrompt == data.PromptsUsed[0])
                        prompt = true;
                }
                else
                {
                    if (data.CurrentPrompt == data.PromptsUsed[0])
                        prompt = true;
                }
                if (!res.MovedForwardSet && data.CurrentStep == 1 && !res.MovedBackPrompt && prompt)
                {
                    //If Step is Step 1 and Prompt is of 0 index, then check for Set move back condition and move it back.
                    //Check the set criterias for  step=1 and  any of the prompts

                    bool bMoveback = false;

                    if (data.FrequencyMoveDown.TotalTrial > 0 && CheckMoveBackFreDur(data, data.FrequencyMoveDown, data.CurrentPrompt, false))
                    {
                        bMoveback = true;
                    }

                    if (data.AvgDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.AvgDurationMoveDown, data.CurrentPrompt, false))
                    {
                        bMoveback = true;
                    }

                    if (data.TotalDurationMoveDown.TotalTrial > 0 && CheckMoveBackAvgTotDur(data, data.TotalDurationMoveDown, data.CurrentPrompt, false))
                    {
                        bMoveback = true;
                    }

                    if (data.MoveBackPercentAccuracy.TotalTrial > 0 && CheckMoveBack(data, data.MoveBackPercentAccuracy, data.CurrentPrompt, false))
                    {
                        bMoveback = true;

                    }
                    if (data.MoveBackPercentIndependence.TotalTrial > 0 && CheckMoveBack(data, data.MoveBackPercentIndependence, data.CurrentPrompt, true))
                    {
                        bMoveback = true;
                    }
                    if (data.SetTotalIncorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.SetTotalIncorrectMoveBack, data.CurrentPrompt, false, false))
                    {
                        bMoveback = true;

                    }
                    if (data.SetTotalCorrectMoveBack.TotalTrial > 0 && CheckMoveBackCorrect(data, data.SetTotalCorrectMoveBack, data.CurrentPrompt, false, true))
                    {
                        bMoveback = true;

                    }
                    if (data.SetLearnedStepMoveBack.TotalTrial > 0 && CheckMoveBack1(data, data.SetLearnedStepMoveBack, data.CurrentPrompt, false))
                    {
                        bMoveback = true;

                    }
                    if (data.SetExcludeCrntStepMoveBack.TotalTrial > 0 && CheckMoveBack2(data, data.SetExcludeCrntStepMoveBack, data.CurrentPrompt, false))
                    {
                        bMoveback = true;

                    }
                    if (data.MoveBackPercentAllIndependence.TotalTrial > 0 && CheckMoveBackPer(data, data.MoveBackPercentAllIndependence, data.CurrentPrompt, false))
                    {
                        bMoveback = true;
                    }
                    if (bMoveback)
                    {
                        res.MovedBackSet = true;
                        if (data.CurrentSet > 0)
                        {
                            res.NextSet = data.CurrentSet - 1;
                            res.MovedBackSet = true;
                        }
                    }
                }
            }


            return res;
        }
        private static float[] GetPromptSuccess(string sPrompt, InputData data)
        {
            float[] fSuccess = new float[data.StepPrompts.Length];
            for (int i = 0; i < data.StepPrompts.Length; i++)
            {
                if (PromptIndex(data.PromptsUsed, data.StepPrompts[i]) >= PromptIndex(data.PromptsUsed, sPrompt))
                {
                    fSuccess[i] = 1;
                }
                else
                {
                    fSuccess[i] = 0;
                }
            }
            return fSuccess;
        }
        public static Result ExecuteForTotalTask(InputData data, bool bStepLevelPrompt, bool bpromptColumn)
        {
            Result res = new Result();
            bool[] bStepMoved = new bool[data.Sessions[0].Steps.Length];
            bool[] bStepMovedBack = new bool[data.Sessions[0].Steps.Length];
            float[] avgCorrect = new float[data.Sessions.Length];
            float[] avgIncorrect = new float[data.Sessions.Length];
            for (int i = 0; i < data.TrialsData.Length; i++)
            {
                avgCorrect[i] = data.TrialsData[i].Correct;
                avgIncorrect[i] = data.TrialsData[i].Incorrect;
            }
            //Get Next Prompts for each set using the criteria
            //if ((data.PromptPercentAccuracy.BarCondition == 0))            
            float FreqScoreSum1 = 0;
            double getTime = 0;
            double AvgDurScoreSum1 = 0;
            double TotDurScoreSum1 = 0;
            if (bStepLevelPrompt)
            {
                for (int j = 0; j < data.Sessions[0].Steps.Length; j++)
                {
                    bool bSuccess = false;
                    bStepMoved[j] = false;
                    int iStartIndex = PromptIndex(data.PromptsUsed, data.StepPrompts[j]);
                    for (int i = 0; i < data.PromptsUsed.Length; i++)
                    {
                        bSuccess = true;
                        float[] avgAcc = new float[data.Sessions.Length];
                        float[] avgInd = new float[data.Sessions.Length];

                        float[] avgPrmFre = new float[data.Sessions.Length];
                        float[] avgPrmAvgDur = new float[data.Sessions.Length];                        
                        float[] avgPrmTotDur = new float[data.Sessions.Length];                        
                        int AvgDurationSum1Count = 0;
                        for (int k = 0; k < data.Sessions.Length; k++)
                        {
                            if (data.Sessions[k].Steps[j].Prompt != null)
                            {
                                if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Prompt) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                {
                                    avgAcc[k] = 1;
                                }
                                else
                                {
                                    avgAcc[k] = 0;
                                }
                                if (data.PromptsUsed.Length != 0)
                                {
                                    if (data.Sessions[k].Steps[j].Prompt == data.PromptsUsed[data.PromptsUsed.Length - 1])
                                    {
                                        avgInd[k] = 1;
                                    }
                                }
                                else
                                {
                                    avgInd[k] = 0;
                                }

                                if (data.PromptFrequencyMoveUp.TotalTrial > 0)
                                {
                                    int GetStat = data.FreDurMoveStat;
                                    string getPrmptScore = "";

                                    if (data.Sessions[k].Steps[j].Prompt != "-1")
                                    {
                                        FreqScoreSum1 = FreqScoreSum1 + float.Parse(data.Sessions[k].Steps[j].Prompt);
                                    }

                                    if (j == ((data.Sessions[k].Steps.Length) - 1))
                                    {
                                        if (GetStat == 1)
                                        {
                                            if (Convert.ToInt32(FreqScoreSum1) < data.PromptFrequencyMoveUp.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                        else if (GetStat == 0)
                                        {
                                            if (Convert.ToInt32(FreqScoreSum1) > data.PromptFrequencyMoveUp.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                    }

                                    if (PromptIndex(data.PromptsUsed, getPrmptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                    {
                                        avgPrmFre[k] = 1;
                                    }
                                    else
                                    {
                                        avgPrmFre[k] = 0;
                                    }
                                }

                                if (data.PromptAvgDurationMoveUp.TotalTrial > 0)
                                {
                                    int GetStat = data.FreDurMoveStat;
                                    string getPrmptScore = "";                                  

                                    if (data.Sessions[k].Steps[j].Prompt != "-1" && data.Sessions[k].Steps[j].Prompt != "00:00:00")
                                    {
                                        AvgDurScoreSum1 = AvgDurScoreSum1 + TimeSpan.Parse(data.Sessions[k].Steps[j].Prompt).TotalSeconds;
                                        AvgDurationSum1Count++;
                                    }

                                    if (j == ((data.Sessions[k].Steps.Length) - 1))
                                    {
                                        if (AvgDurationSum1Count > 0)
                                        {
                                            AvgDurScoreSum1 = AvgDurScoreSum1 / AvgDurationSum1Count;
                                        }
                                        if (GetStat == 1)
                                        {
                                            if (Convert.ToInt32(AvgDurScoreSum1) < data.PromptAvgDurationMoveUp.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                        else if (GetStat == 0)
                                        {
                                            if (Convert.ToInt32(AvgDurScoreSum1) > data.PromptAvgDurationMoveUp.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                    }

                                    if (PromptIndex(data.PromptsUsed, getPrmptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                    {
                                        avgPrmAvgDur[k] = 1;
                                    }
                                    else
                                    {
                                        avgPrmAvgDur[k] = 0;
                                    }
                                }

                                if (data.PromptTotalDurationMoveUp.TotalTrial > 0)
                                {
                                    int GetStat = data.FreDurMoveStat;
                                    string getPrmptScore = "";

                                    if (data.Sessions[k].Steps[j].Prompt != "-1" && data.Sessions[k].Steps[j].Prompt != "00:00:00")
                                    {
                                        TotDurScoreSum1 = TotDurScoreSum1 + TimeSpan.Parse(data.Sessions[k].Steps[j].Prompt).TotalSeconds;
                                    }

                                    if (j == ((data.Sessions[k].Steps.Length) - 1))
                                    {
                                        if (GetStat == 1)
                                        {
                                            if (Convert.ToInt32(TotDurScoreSum1) < data.PromptTotalDurationMoveUp.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                        else if (GetStat == 0)
                                        {
                                            if (Convert.ToInt32(TotDurScoreSum1) > data.PromptTotalDurationMoveUp.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                    }

                                    if (PromptIndex(data.PromptsUsed, getPrmptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                    {
                                        avgPrmTotDur[k] = 1;
                                    }
                                    else
                                    {
                                        avgPrmTotDur[k] = 0;
                                    }
                                } 
                            }
                        }
                        if (data.PromptFrequencyMoveUp.SuccessNeeded > 0)
                        {
                            if (data.PromptFrequencyMoveUp.ConsecutiveAverage)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNew(avgPrmFre, data.PromptFrequencyMoveUp.BarCondition, true, data.PromptFrequencyMoveUp.TotalTrial);
                                if (AccConsecutiveCount < data.PromptFrequencyMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }//--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(End)-- //
                            else if (data.PromptFrequencyMoveUp.ConsecutiveSuccess)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgPrmFre, 1, true);
                                if (AccConsecutiveCount < data.PromptFrequencyMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgPrmFre, 1, data.PromptFrequencyMoveUp.TotalTrial, true);
                                if (AccCount < data.PromptFrequencyMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                        }
                        if (data.PromptAvgDurationMoveUp.SuccessNeeded > 0)
                        {
                            if (data.PromptAvgDurationMoveUp.ConsecutiveAverage)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNew(avgPrmAvgDur, data.PromptAvgDurationMoveUp.BarCondition, true, data.PromptAvgDurationMoveUp.TotalTrial);
                                if (AccConsecutiveCount < data.PromptAvgDurationMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }//--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(End)-- //
                            else if (data.PromptAvgDurationMoveUp.ConsecutiveSuccess)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgPrmAvgDur, 1, true);
                                if (AccConsecutiveCount < data.PromptAvgDurationMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgPrmAvgDur, 1, data.PromptAvgDurationMoveUp.TotalTrial, true);
                                if (AccCount < data.PromptAvgDurationMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                        }
                        if (data.PromptTotalDurationMoveUp.SuccessNeeded > 0)
                        {
                            if (data.PromptTotalDurationMoveUp.ConsecutiveAverage)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNew(avgPrmTotDur, data.PromptTotalDurationMoveUp.BarCondition, true, data.PromptTotalDurationMoveUp.TotalTrial);
                                if (AccConsecutiveCount < data.PromptTotalDurationMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }//--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(End)-- //
                            else if (data.PromptTotalDurationMoveUp.ConsecutiveSuccess)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgPrmTotDur, 1, true);
                                if (AccConsecutiveCount < data.PromptTotalDurationMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgPrmTotDur, 1, data.PromptTotalDurationMoveUp.TotalTrial, true);
                                if (AccCount < data.PromptTotalDurationMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                        }
                        if (data.PromptPercentAccuracy.SuccessNeeded > 0)
                        {
                            //--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(Start)-- //
                            if (data.PromptPercentAccuracy.ConsecutiveAverage)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNew(avgAcc, data.PromptPercentAccuracy.BarCondition, true, data.PromptPercentAccuracy.TotalTrial);
                                if (AccConsecutiveCount < data.PromptPercentAccuracy.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }//--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(End)-- //
                            else if (data.PromptPercentAccuracy.ConsecutiveSuccess)
                            {
                                int AccConsecutiveCount = ConsecutiveCount(avgAcc, 1, true);
                                if (AccConsecutiveCount < data.PromptPercentAccuracy.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCount(avgAcc, 1, data.PromptPercentAccuracy.TotalTrial, true);
                                if (AccCount < data.PromptPercentAccuracy.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                        }

                        if (data.PromptPercentIndependence.SuccessNeeded > 0)
                        {
                            //--- [New Criteria] May 2020 --|PromptPercentIndependence|--(Start)-- //
                            if (data.PromptPercentIndependence.ConsecutiveAverage)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNew(avgAcc, data.PromptPercentIndependence.BarCondition, true, data.PromptPercentIndependence.TotalTrial);
                                if (AccConsecutiveCount < data.PromptPercentIndependence.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }//--- [New Criteria] May 2020 --|PromptPercentIndependence|--(End)-- //
                            else if (data.PromptPercentIndependence.ConsecutiveSuccess)
                            {
                                int AccConsecutiveCount = ConsecutiveCount(avgInd, 1, true);
                                if (AccConsecutiveCount < data.PromptPercentIndependence.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCount(avgInd, 1, data.PromptPercentIndependence.TotalTrial, true);
                                if (AccCount < data.PromptPercentIndependence.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                        }
                        if (data.PromptTotalCorrectMoveUp.SuccessNeeded > 0)
                        {
                            //--- [New Criteria] May 2020 --|PromptTotalCorrectMoveUp|--(Start)-- //
                            if (data.PromptTotalCorrectMoveUp.ConsecutiveAverage)
                            {
                                int TCConsecutiveCount = ConsecutiveCountCorrectTestNew(avgAcc, 1, true);
                                if (TCConsecutiveCount < data.PromptTotalCorrectMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }//--- [New Criteria] May 2020 --|PromptTotalCorrectMoveUp|--(End)-- //
                            else if (data.PromptTotalCorrectMoveUp.ConsecutiveSuccess)
                            {
                                int TCConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, 1, true);
                                if (TCConsecutiveCount < data.PromptTotalCorrectMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                            else
                            {
                                int TCCount = SuccessORFailureCountCorrect(avgCorrect, 1, data.PromptTotalCorrectMoveUp.TotalTrial, true);
                                if (TCCount < data.PromptTotalCorrectMoveUp.SuccessNeeded)
                                {
                                    bSuccess = false;
                                }
                            }
                        }

                        if (!bSuccess)
                        {
                            if (PromptIndex(data.PromptsUsed, data.PromptsUsed[i]) > PromptIndex(data.PromptsUsed, data.StepPrompts[j]))
                            {
                                data.StepPrompts[j] = data.PromptsUsed[i];
                                res.MoveForwardPromptStep = true;
                                bStepMoved[j] = true;
                            }
                            else
                            {
                                ////Move back only if it meets move back criteria. Commenting below code
                                //if (PromptIndex(data.PromptsUsed, data.StepPrompts[j]) > 0)
                                //    data.StepPrompts[j] = data.PromptsUsed[PromptIndex(data.PromptsUsed, data.StepPrompts[j]) - 1];
                            }
                            break;
                        }

                    }
                    if (bSuccess)
                    {
                        data.StepPrompts[j] = data.TargetPrompt;
                        res.MoveForwardPromptStep = true;
                        bStepMoved[j] = true;
                    }
                }
                res.StepPrompts = data.StepPrompts;


                //Check Prompt Move Back...
                for (int j = 0; j < data.Sessions[0].Steps.Length; j++)
                {
                    bool bSuccess = false;
                    bStepMovedBack[j] = false;
                    if (!bStepMoved[j])
                    {
                        int iPromptIndex = PromptIndex(data.PromptsUsed, data.StepPrompts[j]);
                        float[] avgAcc = new float[data.Sessions.Length];

                        float[] avgPrmFre = new float[data.Sessions.Length];
                        float[] avgPrmAvgDur = new float[data.Sessions.Length];
                        float[] avgPrmTotDur = new float[data.Sessions.Length];
                        int AvgDurationSum1Count = 0;
                        for (int k = 0; k < data.Sessions.Length; k++)
                        {
                            if (data.Sessions[k].Steps[j].Prompt != null)
                            {
                                if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Prompt) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[iPromptIndex]))
                                {
                                    avgAcc[k] = 1;
                                }
                                else
                                {
                                    avgAcc[k] = 0;
                                }

                                if (data.PromptFrequencyMoveDown.TotalTrial > 0)
                                {
                                    int GetStat = data.FreDurMoveStat;
                                    string getPrmptScore = "";

                                    if (data.Sessions[k].Steps[j].Prompt != "-1")
                                    {
                                        FreqScoreSum1 = FreqScoreSum1 + float.Parse(data.Sessions[k].Steps[j].Prompt);
                                    }

                                    if (j == ((data.Sessions[k].Steps.Length) - 1))
                                    {
                                        if (GetStat == 1)
                                        {
                                            if (Convert.ToInt32(FreqScoreSum1) < data.PromptFrequencyMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                        else if (GetStat == 0)
                                        {
                                            if (Convert.ToInt32(FreqScoreSum1) > data.PromptFrequencyMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                    }

                                    if (PromptIndex(data.PromptsUsed, getPrmptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[iPromptIndex]))
                                    {
                                        avgPrmFre[k] = 1;
                                    }
                                    else
                                    {
                                        avgPrmFre[k] = 0;
                                    }
                                }
                                if (data.PromptAvgDurationMoveDown.TotalTrial > 0)
                                {
                                    int GetStat = data.FreDurMoveStat;
                                    string getPrmptScore = "";

                                    if (data.Sessions[k].Steps[j].Prompt != "-1" && data.Sessions[k].Steps[j].Prompt != "00:00:00")
                                    {
                                        AvgDurScoreSum1 = AvgDurScoreSum1 + TimeSpan.Parse(data.Sessions[k].Steps[j].Prompt).TotalSeconds;
                                        AvgDurationSum1Count++;
                                    }

                                    if (j == ((data.Sessions[k].Steps.Length) - 1))
                                    {
                                        if (AvgDurationSum1Count > 0)
                                        {
                                            AvgDurScoreSum1 = AvgDurScoreSum1 / AvgDurationSum1Count;
                                        }
                                        if (GetStat == 1)
                                        {
                                            if (Convert.ToInt32(AvgDurScoreSum1) < data.PromptAvgDurationMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                        else if (GetStat == 0)
                                        {
                                            if (Convert.ToInt32(AvgDurScoreSum1) > data.PromptAvgDurationMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                    }

                                    if (PromptIndex(data.PromptsUsed, getPrmptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[iPromptIndex]))
                                    {
                                        avgPrmAvgDur[k] = 1;
                                    }
                                    else
                                    {
                                        avgPrmAvgDur[k] = 0;
                                    }
                                }
                                if (data.PromptTotalDurationMoveDown.TotalTrial > 0)
                                {
                                    int GetStat = data.FreDurMoveStat;
                                    string getPrmptScore = "";

                                    if (data.Sessions[k].Steps[j].Prompt != "-1" && data.Sessions[k].Steps[j].Prompt != "00:00:00")
                                    {
                                        TotDurScoreSum1 = TotDurScoreSum1 + TimeSpan.Parse(data.Sessions[k].Steps[j].Prompt).TotalSeconds;
                                    }

                                    if (j == ((data.Sessions[k].Steps.Length) - 1))
                                    {
                                        if (GetStat == 1)
                                        {
                                            if (Convert.ToInt32(TotDurScoreSum1) < data.PromptTotalDurationMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                        else if (GetStat == 0)
                                        {
                                            if (Convert.ToInt32(TotDurScoreSum1) > data.PromptTotalDurationMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                    }

                                    if (PromptIndex(data.PromptsUsed, getPrmptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[iPromptIndex]))
                                    {
                                        avgPrmTotDur[k] = 1;
                                    }
                                    else
                                    {
                                        avgPrmTotDur[k] = 0;
                                    }
                                }
                            }
                        }
                        bool bStepPromptMoveBack = true;
                        if (data.PromptFrequencyMoveDown.FailureNeeded> 0)
                        {
                            //--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(Start)-- //
                            if (data.PromptFrequencyMoveDown.ConsecutiveAverageFailure)
                            {
                                int TCConsecutiveCount = ConsecutiveCountTestNew(avgPrmFre, data.PromptFrequencyMoveDown.BarCondition, false, data.PromptFrequencyMoveDown.TotalTrial);
                                if (TCConsecutiveCount < data.PromptFrequencyMoveDown.FailureNeeded)
                                {
                                    bStepPromptMoveBack = false;
                                }
                            }//--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(End)-- //
                            else if (data.PromptFrequencyMoveDown.ConsecutiveFailures)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgPrmFre, 0, false);
                                if (AccConsecutiveCount < data.PromptFrequencyMoveDown.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgPrmFre, 0, data.PromptFrequencyMoveDown.TotalTrial, false);
                                if (AccCount < data.PromptFrequencyMoveDown.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                        }
                        if (data.PromptAvgDurationMoveDown.FailureNeeded > 0)
                        {
                            //--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(Start)-- //
                            if (data.PromptAvgDurationMoveDown.ConsecutiveAverageFailure)
                            {
                                int TCConsecutiveCount = ConsecutiveCountTestNew(avgPrmAvgDur, data.PromptAvgDurationMoveDown.BarCondition, false, data.PromptAvgDurationMoveDown.TotalTrial);
                                if (TCConsecutiveCount < data.PromptAvgDurationMoveDown.FailureNeeded)
                                {
                                    bStepPromptMoveBack = false;
                                }
                            }//--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(End)-- //
                            else if (data.PromptAvgDurationMoveDown.ConsecutiveFailures)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgPrmAvgDur, 0, false);
                                if (AccConsecutiveCount < data.PromptAvgDurationMoveDown.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgPrmAvgDur, 0, data.PromptAvgDurationMoveDown.TotalTrial, false);
                                if (AccCount < data.PromptAvgDurationMoveDown.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                        }
                        if (data.PromptTotalDurationMoveDown.FailureNeeded > 0)
                        {
                            //--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(Start)-- //
                            if (data.PromptAvgDurationMoveDown.ConsecutiveAverageFailure)
                            {
                                int TCConsecutiveCount = ConsecutiveCountTestNew(avgPrmTotDur, data.PromptTotalDurationMoveDown.BarCondition, false, data.PromptTotalDurationMoveDown.TotalTrial);
                                if (TCConsecutiveCount < data.PromptTotalDurationMoveDown.FailureNeeded)
                                {
                                    bStepPromptMoveBack = false;
                                }
                            }//--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(End)-- //
                            else if (data.PromptTotalDurationMoveDown.ConsecutiveFailures)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgPrmTotDur, 0, false);
                                if (AccConsecutiveCount < data.PromptTotalDurationMoveDown.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgPrmTotDur, 0, data.PromptTotalDurationMoveDown.TotalTrial, false);
                                if (AccCount < data.PromptTotalDurationMoveDown.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                        }
                        if (data.MoveBackPromptPercentAccuracy.FailureNeeded > 0)
                        {
                            //--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(Start)-- //
                            if (data.MoveBackPromptPercentAccuracy.ConsecutiveAverageFailure)
                            {
                                int TCConsecutiveCount = ConsecutiveCountTestNew(avgAcc, data.MoveBackPromptPercentAccuracy.BarCondition, false, data.MoveBackPromptPercentAccuracy.TotalTrial);
                                if (TCConsecutiveCount < data.MoveBackPromptPercentAccuracy.FailureNeeded)
                                {
                                    bStepPromptMoveBack = false;
                                }
                            }//--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(End)-- //
                            else if (data.MoveBackPromptPercentAccuracy.ConsecutiveFailures)
                            {
                                int AccConsecutiveCount = ConsecutiveCount(avgAcc, 0, false);
                                if (AccConsecutiveCount < data.MoveBackPromptPercentAccuracy.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCount(avgAcc, 0, data.MoveBackPromptPercentAccuracy.TotalTrial, false);
                                if (AccCount < data.MoveBackPromptPercentAccuracy.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                        }
                        if (data.MoveBackPromptPercentIndependence.FailureNeeded > 0)
                        {
                            //--- [New Criteria] May 2020 --|MoveBackPromptPercentIndependence|--(Start)-- //
                            if (data.MoveBackPromptPercentIndependence.ConsecutiveAverageFailure)
                            {
                                int TCConsecutiveCount = ConsecutiveCountTestNew(avgAcc, data.MoveBackPromptPercentIndependence.BarCondition, false, data.MoveBackPromptPercentIndependence.TotalTrial);
                                if (TCConsecutiveCount < data.MoveBackPromptPercentIndependence.FailureNeeded)
                                {
                                    bStepPromptMoveBack = false;
                                }
                            }//--- [New Criteria] May 2020 --|MoveBackPromptPercentIndependence|--(End)-- //
                            else if (data.MoveBackPromptPercentIndependence.ConsecutiveFailures)
                            {
                                int AccConsecutiveCount = ConsecutiveCount(avgAcc, 0, false);
                                if (AccConsecutiveCount < data.MoveBackPromptPercentIndependence.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCount(avgAcc, 0, data.MoveBackPromptPercentIndependence.TotalTrial, false);
                                if (AccCount < data.MoveBackPromptPercentIndependence.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                        }
                        if (data.PromptTotalIncorrectMoveBack.FailureNeeded > 0)
                        {
                            //--- [New Criteria] May 2020 --|PromptTotalIncorrectMoveBack|--(Start)-- //
                            if (data.PromptTotalIncorrectMoveBack.ConsecutiveAverageFailure)
                            {
                                int TCConsecutiveCount = ConsecutiveCountCorrectTestNew(avgIncorrect, 0, false);
                                if (TCConsecutiveCount < data.PromptTotalIncorrectMoveBack.FailureNeeded)
                                {
                                    bStepPromptMoveBack = false;
                                }
                            }//--- [New Criteria] May 2020 --|PromptTotalIncorrectMoveBack|--(End)-- //
                            else if (data.PromptTotalIncorrectMoveBack.ConsecutiveFailures)
                            {
                                int TCConsecutiveCount = ConsecutiveCountCorrect(avgIncorrect, 0, false);
                                if (TCConsecutiveCount < data.PromptTotalIncorrectMoveBack.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                            else
                            {
                                int TCCount = SuccessORFailureCountCorrect(avgIncorrect, 0, data.PromptTotalIncorrectMoveBack.TotalTrial, false);
                                if (TCCount < data.PromptTotalIncorrectMoveBack.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                        }
                        if (data.PromptTotalCorrectMoveBack.FailureNeeded > 0)
                        {
                            //--- [New Criteria] May 2020 --|PromptTotalCorrectMoveBack|--(Start)-- //
                            if (data.PromptTotalCorrectMoveBack.ConsecutiveAverageFailure)
                            {
                                int TCConsecutiveCount = ConsecutiveCountCorrectTestNew(avgCorrect, 0, false);
                                if (TCConsecutiveCount < data.PromptTotalCorrectMoveBack.FailureNeeded)
                                {
                                    bStepPromptMoveBack = false;
                                }
                            }//--- [New Criteria] May 2020 --|PromptTotalCorrectMoveBack|--(End)-- //
                            else if (data.PromptTotalCorrectMoveBack.ConsecutiveFailures)
                            {
                                int TCConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, 0, false);
                                if (TCConsecutiveCount < data.PromptTotalCorrectMoveBack.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                            else
                            {
                                int TCCount = SuccessORFailureCountCorrect(avgCorrect, 0, data.PromptTotalCorrectMoveBack.TotalTrial, false);
                                if (TCCount < data.PromptTotalCorrectMoveBack.FailureNeeded)
                                    bStepPromptMoveBack = false;
                            }
                        }

                        if (bStepPromptMoveBack)
                        {
                            bStepMovedBack[j] = true;
                            if (PromptIndex(data.PromptsUsed, data.StepPrompts[j]) > 0)
                            {
                                res.StepPrompts[j] = data.PromptsUsed[PromptIndex(data.PromptsUsed, data.StepPrompts[j]) - 1];
                                res.MoveBackPromptStep = true;
                            }

                        }

                    }
                }

            }

            float[] promptSuccess = GetPromptSuccess(data.CurrentPrompt, data);
            if (data.PromptsUsed.Length != 0)
            {
                float[] indPromptSuccess = GetPromptSuccess(data.PromptsUsed[data.PromptsUsed.Length - 1], data);
            }
            float[] avgAccSet = null;
            float[] avgIndSet = null;

            float[] avgSetFre = null;
            float[] avgSetFreNew = null;
            float[] avgSetAvgDur = null;
            float[] avgSetAvgDurNew = null;
            float[] avgSetTotDur = null;
            float[] avgSetTotDurNew = null;

            //Check for Set Move Criterias

            bool bPromptSuccess = false;
            //if ((data.PromptPercentAccuracy.BarCondition > 0) && (data.PromptPercentAccuracy.BarCondition < 100))
            if (!bStepLevelPrompt)
            {
                avgAccSet = new float[data.Sessions.Length];
                avgIndSet = new float[data.Sessions.Length];

                avgSetFre = new float[data.Sessions.Length];
                avgSetAvgDur = new float[data.Sessions.Length];
                avgSetAvgDurNew = new float[data.Sessions.Length];

                avgSetFreNew = new float[data.Sessions.Length];

                avgSetTotDur = new float[data.Sessions.Length];
                avgSetTotDurNew = new float[data.Sessions.Length];

                //Check Prompt MoveUp
                int iStartIndex = PromptIndex(data.PromptsUsed, data.CurrentPrompt);

                for (int i = iStartIndex; i < data.PromptsUsed.Length; i++)
                {
                    for (int k = 0; k < data.Sessions.Length; k++)
                    {
                        float AccCount = 0;
                        float IndCount = 0;

                        float FreCount = 0;
                        float AvgDurCount = 0;
                        float TotDurCount = 0;

                        float FreqScoreSum = 0;
                        int FreqScoreSumCount = 0;
                        double AvgDurScoreSum = 0;
                        int AvgDurScoreSumCount = 0;
                        double TotDurScoreSum = 0;
                        int TotDurScoreSumCount = 0;
                        for (int j = 0; j < data.Sessions[k].Steps.Length; j++)
                        {
                            if (data.Sessions[k].Steps[j].Prompt != null)
                            {
                                if (data.PromptsUsed.Length != 0)
                                {
                                    if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Prompt) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                    {
                                        AccCount++;
                                    }
                                    if (data.Sessions[k].Steps[j].Prompt == data.PromptsUsed[data.PromptsUsed.Length - 1])
                                    {
                                        IndCount++;
                                    }

                                    if (data.PromptFrequencyMoveUp.TotalTrial > 0)
                                    {
                                        int GetStat = data.FreDurMoveStat;
                                        string getSetScore = "";
                                        if (data.Sessions[k].Steps[j].Prompt != "-1")
                                        {
                                            FreqScoreSum = FreqScoreSum + float.Parse(data.Sessions[k].Steps[j].Prompt);
                                            FreqScoreSumCount++;
                                        }

                                        if (j == ((data.Sessions[k].Steps.Length)-1))
                                        {
                                            if (GetStat == 1)
                                            {
                                                if (Convert.ToInt32(FreqScoreSum) < data.PromptFrequencyMoveUp.BarCondition)
                                                {
                                                    getSetScore = "+";
                                }
                                                else
                                                {
                                                    getSetScore = "-";
                            }
                        }
                                            else if (GetStat == 0)
                                            {
                                                if (Convert.ToInt32(FreqScoreSum) > data.PromptFrequencyMoveUp.BarCondition)
                                                {
                                                    getSetScore = "+";
                                                }
                                                else
                                                {
                                                    getSetScore = "-";
                                                }
                                            }
                                        }

                                        if (PromptIndex(data.PromptsUsed, getSetScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                        {
                                            FreCount++;
                                        }
                                    }
                                    if (data.PromptAvgDurationMoveUp.TotalTrial > 0)
                                    {
                                        int GetStat = data.FreDurMoveStat;
                                        string getPrmptScore = "";

                                        if (data.Sessions[k].Steps[j].Prompt != "-1" && data.Sessions[k].Steps[j].Prompt != "00:00:00")
                                        {
                                            AvgDurScoreSum = AvgDurScoreSum + TimeSpan.Parse(data.Sessions[k].Steps[j].Prompt).TotalSeconds;
                                            AvgDurScoreSumCount++;
                                        }

                                        if (j == ((data.Sessions[k].Steps.Length) - 1))
                                        {
                                            if (AvgDurScoreSumCount > 0)
                                            {
                                                AvgDurScoreSum = AvgDurScoreSum / AvgDurScoreSumCount;
                                            }
                                            if (GetStat == 1)
                                            {
                                                if (Convert.ToInt32(AvgDurScoreSum) < data.PromptAvgDurationMoveUp.BarCondition)
                                                {
                                                    getPrmptScore = "+";
                                                }
                                                else
                                                {
                                                    getPrmptScore = "-";
                                                }
                                            }
                                            else if (GetStat == 0)
                                            {
                                                if (Convert.ToInt32(AvgDurScoreSum) > data.PromptAvgDurationMoveUp.BarCondition)
                                                {
                                                    getPrmptScore = "+";
                                                }
                                                else
                                                {
                                                    getPrmptScore = "-";
                                                }
                                            }
                                        }

                                        if (PromptIndex(data.PromptsUsed, getPrmptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                        {
                                            AvgDurCount++;
                                        }
                                    }

                                    if (data.PromptTotalDurationMoveUp.TotalTrial > 0)
                                    {
                                        int GetStat = data.FreDurMoveStat;
                                        string getPrmptScore = "";

                                        if (data.Sessions[k].Steps[j].Prompt != "-1" && data.Sessions[k].Steps[j].Prompt != "00:00:00")
                                        {
                                            TotDurScoreSum = TotDurScoreSum + TimeSpan.Parse(data.Sessions[k].Steps[j].Prompt).TotalSeconds;
                                            TotDurScoreSumCount++;
                                        }

                                        if (j == ((data.Sessions[k].Steps.Length) - 1))
                                        {
                                            if (GetStat == 1)
                                            {
                                                if (Convert.ToInt32(TotDurScoreSum) < data.PromptTotalDurationMoveUp.BarCondition)
                                                {
                                                    getPrmptScore = "+";
                                                }
                                                else
                                                {
                                                    getPrmptScore = "-";
                                                }
                                            }
                                            else if (GetStat == 0)
                                            {
                                                if (Convert.ToInt32(TotDurScoreSum) > data.PromptTotalDurationMoveUp.BarCondition)
                                                {
                                                    getPrmptScore = "+";
                                                }
                                                else
                                                {
                                                    getPrmptScore = "-";
                                                }
                                            }
                                        }

                                        if (PromptIndex(data.PromptsUsed, getPrmptScore) >= PromptIndex(data.PromptsUsed, data.PromptsUsed[i]))
                                        {
                                            TotDurCount++;
                                        }
                                    } 
                                }
                            }
                        }
                        avgAccSet[k] = AccCount / data.Sessions[k].GetNoScoredSessionCount();
                        avgIndSet[k] = IndCount / data.Sessions[k].GetNoScoredSessionCount();

                        avgSetFre[k] = (FreCount * FreqScoreSumCount) / data.Sessions[k].GetNoScoredFreqDurSessionCount();
                        float freqscore = (float)FreqScoreSum;
                        avgSetFreNew[k] = freqscore;
                        avgSetAvgDur[k] = (AvgDurCount * AvgDurScoreSumCount) / data.Sessions[k].GetNoScoredFreqDurSessionCount();
                        float avgdur = (float)AvgDurScoreSum;
                        avgSetAvgDurNew[k] = avgdur;
                        avgSetTotDur[k] = (TotDurCount * TotDurScoreSumCount) / data.Sessions[k].GetNoScoredFreqDurSessionCount();
                        float totDur = (float)TotDurScoreSum;
                        avgSetTotDurNew[k] = totDur;
                    }
                    if (data.PromptFrequencyMoveUp.TotalTrial > 0)
                    {
                        bPromptSuccess = true;
                        //--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(Start)-- //
                        if (data.PromptFrequencyMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetFreNew, data.PromptFrequencyMoveUp.BarCondition, true, data.PromptFrequencyMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.PromptFrequencyMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(End)-- //
                        else if (data.PromptFrequencyMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetFre, data.PromptFrequencyMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.PromptFrequencyMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgSetFre, data.PromptFrequencyMoveUp.BarCondition, data.PromptFrequencyMoveUp.TotalTrial, true);
                            if (AccCount < data.PromptFrequencyMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                    }
                    if (data.PromptAvgDurationMoveUp.TotalTrial > 0)
                    {
                        bPromptSuccess = true;
                        //--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(Start)-- //
                        if (data.PromptAvgDurationMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetAvgDurNew, data.PromptAvgDurationMoveUp.BarCondition, true, data.PromptAvgDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.PromptAvgDurationMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(End)-- //
                        else if (data.PromptAvgDurationMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetAvgDur, data.PromptAvgDurationMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.PromptAvgDurationMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgSetAvgDur, data.PromptAvgDurationMoveUp.BarCondition, data.PromptAvgDurationMoveUp.TotalTrial, true);
                            if (AccCount < data.PromptAvgDurationMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                    }
                    if (data.PromptTotalDurationMoveUp.TotalTrial > 0)
                    {
                        bPromptSuccess = true;
                        //--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(Start)-- //
                        if (data.PromptTotalDurationMoveUp.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetTotDurNew, data.PromptTotalDurationMoveUp.BarCondition, true, data.PromptTotalDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.PromptTotalDurationMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(End)-- //
                        else if (data.PromptTotalDurationMoveUp.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetTotDur, data.PromptTotalDurationMoveUp.BarCondition, true);
                            if (AccConsecutiveCount < data.PromptTotalDurationMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgSetTotDur, data.PromptTotalDurationMoveUp.BarCondition, data.PromptTotalDurationMoveUp.TotalTrial, true);
                            if (AccCount < data.PromptTotalDurationMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                    }
                    if (data.PromptPercentAccuracy.TotalTrial > 0)
                    {
                        bPromptSuccess = true;
                        //--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(Start)-- //
                        if (data.PromptPercentAccuracy.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.PromptPercentAccuracy.BarCondition, true, data.PromptPercentAccuracy.TotalTrial);
                            if (AccConsecutiveCount < data.PromptPercentAccuracy.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }//--- [New Criteria] May 2020 --|PromptPercentAccuracy|--(End)-- //
                        else if (data.PromptPercentAccuracy.ConsecutiveSuccess)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgAccSet, data.PromptPercentAccuracy.BarCondition, true);
                            if (AccConsecutiveCount < data.PromptPercentAccuracy.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }                        
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgAccSet, data.PromptPercentAccuracy.BarCondition, data.PromptPercentAccuracy.TotalTrial, true);
                            if (AccCount < data.PromptPercentAccuracy.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                    }
                    if (data.PromptPercentIndependence.TotalTrial > 0)
                    {
                        bPromptSuccess = true;
                        if (data.PromptPercentIndependence.ConsecutiveAverage)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.PromptPercentIndependence.BarCondition, true,data.MoveBackPromptPercentIndependence.TotalTrial);
                            if (AccConsecutiveCount < data.PromptPercentIndependence.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                        else if (data.PromptPercentIndependence.ConsecutiveSuccess)
                        {
                            int IndConsecutiveCount = ConsecutiveCount(avgAccSet, data.PromptPercentIndependence.BarCondition, true);
                            if (IndConsecutiveCount < data.PromptPercentIndependence.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                        else
                        {
                            int IndCount = SuccessORFailureCount(avgAccSet, data.PromptPercentIndependence.BarCondition, data.PromptPercentIndependence.TotalTrial, true);
                            if (IndCount < data.PromptPercentIndependence.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                    }
                    if (data.PromptTotalCorrectMoveUp.TotalTrial > 0)
                    {
                        bPromptSuccess = true;
                        if (data.PromptTotalCorrectMoveUp.ConsecutiveAverage)
                        {
                            int TCConsecutiveCount = ConsecutiveCountTestNewCorrect(avgCorrect, data.PromptTotalCorrectMoveUp.BarCondition, true, data.PromptTotalCorrectMoveUp.TotalTrial);
                            if (TCConsecutiveCount < data.PromptTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                        else if (data.PromptTotalCorrectMoveUp.ConsecutiveSuccess)
                        {
                            int TCConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, data.PromptTotalCorrectMoveUp.BarCondition, true);
                            if (TCConsecutiveCount < data.PromptTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                        else
                        {
                            int TCCount = SuccessORFailureCountCorrect(avgCorrect, data.PromptTotalCorrectMoveUp.BarCondition, data.PromptTotalCorrectMoveUp.TotalTrial, true);
                            if (TCCount < data.PromptTotalCorrectMoveUp.SuccessNeeded)
                            {
                                bPromptSuccess = false;
                            }
                        }
                    }
                    if (bPromptSuccess)
                    {
                        //Move Prompt
                        int iNewPromptIndex = i + 1;
                        if (!bpromptColumn)
                        {
                            if (data.PromptHirecharchy)
                            {
                                if (data.NoPromptsUsed.Length > 0)
                                {
                                    if (iNewPromptIndex < data.NoPromptsUsed.Length)
                                    {
                                        res.NextPrompt = data.NoPromptsUsed[iNewPromptIndex];
                                        res.MovedForwardPrompt = true;
                                    }
                                }
                            }
                        }
                        else
                        {

                            if (iNewPromptIndex < data.PromptsUsed.Length)
                            {
                                res.NextPrompt = data.PromptsUsed[iNewPromptIndex];
                                res.MovedForwardPrompt = true;
                            }
                        }

                    }
                }
                //If there are no Prompt MoveUp then check for MoveDown
                if (res.MovedForwardPrompt == false)
                {

                    //Get the accuracy array for current prompt
                    for (int k = 0; k < data.Sessions.Length; k++)
                    {
                        float AccCount = 0;
                        float IndCount = 0;

                        float FreCount = 0;
                        float AvgDurCount = 0;
                        float TotDurCount = 0;
                        float FreqScoreSum = 0;
                        int FreqScoreSumCount = 0;
                        double AvgDurScoreSum = 0;
                        int AvgDurScoreSumCount = 0;
                        double TotDurScoreSum = 0;
                        int TotDurScoreSumCount = 0;

                        for (int j = 0; j < data.Sessions[k].Steps.Length; j++)
                        {
                            if (data.Sessions[k].Steps[j].Prompt != null)
                            {
                                if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Prompt) >= PromptIndex(data.PromptsUsed, data.CurrentPrompt))
                                {
                                    AccCount++;
                                }
                                if (data.PromptsUsed.Length != 0)
                                {
                                    if (data.Sessions[k].Steps[j].Prompt == data.PromptsUsed[data.PromptsUsed.Length - 1])
                                    {
                                        IndCount++;
                                    }
                                }

                                if (data.PromptFrequencyMoveDown.TotalTrial > 0)
                                {
                                    int GetStat = data.FreDurMoveStat;
                                    string getPrmptScore = "";

                                    if (data.Sessions[k].Steps[j].Prompt != "-1")
                                    {
                                        FreqScoreSum = FreqScoreSum + float.Parse(data.Sessions[k].Steps[j].Prompt);
                                        FreqScoreSumCount++;
                                    }

                                    if (j == ((data.Sessions[k].Steps.Length) - 1))
                                    {
                                        if (GetStat == 1)
                                        {
                                            if (Convert.ToInt32(FreqScoreSum) < data.PromptFrequencyMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                        }
                                        }
                                        else if (GetStat == 0)
                                        {
                                            if (Convert.ToInt32(FreqScoreSum) > data.PromptFrequencyMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                    }

                                    if (PromptIndex(data.PromptsUsed, getPrmptScore) >= PromptIndex(data.PromptsUsed, data.CurrentPrompt))
                                    {
                                        FreCount++;
                                    }
                                }
                                if (data.PromptAvgDurationMoveDown.TotalTrial > 0)
                                {
                                    int GetStat = data.FreDurMoveStat;
                                    string getPrmptScore = "";

                                    if (data.Sessions[k].Steps[j].Prompt != "-1" && data.Sessions[k].Steps[j].Prompt != "00:00:00")
                                    {
                                        AvgDurScoreSum = AvgDurScoreSum + TimeSpan.Parse(data.Sessions[k].Steps[j].Prompt).TotalSeconds;
                                        AvgDurScoreSumCount++;
                                    }

                                    if (j == ((data.Sessions[k].Steps.Length) - 1))
                                    {
                                        if (AvgDurScoreSumCount > 0)
                                        {
                                            AvgDurScoreSum = AvgDurScoreSum / AvgDurScoreSumCount;
                                        }
                                        if (GetStat == 1)
                                        {
                                            if (Convert.ToInt32(AvgDurScoreSum) < data.PromptAvgDurationMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                        else if (GetStat == 0)
                                        {
                                            if (Convert.ToInt32(AvgDurScoreSum) > data.PromptAvgDurationMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                    }

                                    if (PromptIndex(data.PromptsUsed, getPrmptScore) >= PromptIndex(data.PromptsUsed, data.CurrentPrompt))
                                    {
                                        AvgDurCount++;
                                    }
                                }

                                if (data.PromptTotalDurationMoveDown.TotalTrial > 0)
                                {
                                    int GetStat = data.FreDurMoveStat;
                                    string getPrmptScore = "";

                                    if (data.Sessions[k].Steps[j].Prompt != "-1" && data.Sessions[k].Steps[j].Prompt != "00:00:00")
                                    {
                                        TotDurScoreSum = TotDurScoreSum + TimeSpan.Parse(data.Sessions[k].Steps[j].Prompt).TotalSeconds;
                                        TotDurScoreSumCount++;
                                    }

                                    if (j == ((data.Sessions[k].Steps.Length) - 1))
                                    {
                                        if (GetStat == 1)
                                        {
                                            if (Convert.ToInt32(TotDurScoreSum) < data.PromptTotalDurationMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                        else if (GetStat == 0)
                                        {
                                            if (Convert.ToInt32(TotDurScoreSum) > data.PromptTotalDurationMoveDown.BarCondition)
                                            {
                                                getPrmptScore = "+";
                                            }
                                            else
                                            {
                                                getPrmptScore = "-";
                                            }
                                        }
                                    }

                                    if (PromptIndex(data.PromptsUsed, getPrmptScore) >= PromptIndex(data.PromptsUsed, data.CurrentPrompt))
                                    {
                                        TotDurCount++;
                                    }
                                } 
                            }
                        }
                        avgAccSet[k] = AccCount / data.Sessions[k].GetNoScoredSessionCount();
                        avgIndSet[k] = IndCount / data.Sessions[k].GetNoScoredSessionCount();

                        avgSetFre[k] = (FreCount * FreqScoreSumCount) / data.Sessions[k].GetNoScoredFreqDurSessionCount();
                        float freqscore = FreqScoreSum;
                        avgSetFreNew[k] = freqscore;
                        avgSetAvgDur[k] = (AvgDurCount * AvgDurScoreSumCount) / data.Sessions[k].GetNoScoredFreqDurSessionCount();
                        float avgdur = (float) AvgDurScoreSum;
                        avgSetAvgDurNew[k] = avgdur;
                        avgSetTotDur[k] = (TotDurCount * TotDurScoreSumCount) / data.Sessions[k].GetNoScoredFreqDurSessionCount();
                        float totDur = (float)TotDurScoreSum;
                        avgSetTotDurNew[k] = totDur;
                    }

                    bool bPromptMoveBack = false;
                    //Check for Set Move Back
                    if (data.PromptFrequencyMoveDown.TotalTrial > 0)
                    {
                        bPromptMoveBack = true;
                        //--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(Start)-- //
                        if (data.PromptFrequencyMoveDown.ConsecutiveAverageFailure)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetFreNew, data.PromptFrequencyMoveDown.BarCondition, false, data.PromptFrequencyMoveDown.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.PromptFrequencyMoveDown.FailureNeeded)
                                bPromptMoveBack = false;
                        }//--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(End)-- //
                        else if (data.PromptFrequencyMoveDown.ConsecutiveFailures)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetFre, data.PromptFrequencyMoveDown.BarCondition, false);
                            if (AccConsecutiveCount < data.PromptFrequencyMoveDown.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgSetFre, data.PromptFrequencyMoveDown.BarCondition, data.PromptFrequencyMoveDown.TotalTrial, false);
                            if (AccCount < data.PromptFrequencyMoveDown.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                    }
                    if (data.PromptAvgDurationMoveDown.TotalTrial > 0)
                    {
                        bPromptMoveBack = true;
                        //--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(Start)-- //
                        if (data.PromptAvgDurationMoveDown.ConsecutiveAverageFailure)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetAvgDurNew, data.PromptAvgDurationMoveDown.BarCondition, false, data.PromptAvgDurationMoveDown.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.PromptAvgDurationMoveDown.FailureNeeded)
                                bPromptMoveBack = false;
                        }//--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(End)-- //
                        else if (data.PromptAvgDurationMoveDown.ConsecutiveFailures)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetAvgDur, data.PromptAvgDurationMoveDown.BarCondition, false);
                            if (AccConsecutiveCount < data.PromptAvgDurationMoveDown.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgSetAvgDur, data.PromptAvgDurationMoveDown.BarCondition, data.PromptAvgDurationMoveDown.TotalTrial, false);
                            if (AccCount < data.PromptAvgDurationMoveDown.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                    }
                    if (data.PromptTotalDurationMoveDown.TotalTrial > 0)
                    {
                        bPromptMoveBack = true;
                        //--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(Start)-- //
                        if (data.PromptTotalDurationMoveDown.ConsecutiveAverageFailure)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetTotDurNew, data.PromptTotalDurationMoveDown.BarCondition, false, data.PromptTotalDurationMoveDown.TotalTrial, data.FreDurMoveStat);
                            if (AccConsecutiveCount < data.PromptTotalDurationMoveDown.FailureNeeded)
                                bPromptMoveBack = false;
                        }//--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(End)-- //
                        else if (data.PromptTotalDurationMoveDown.ConsecutiveFailures)
                        {
                            int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetTotDur, data.PromptTotalDurationMoveDown.BarCondition, false);
                            if (AccConsecutiveCount < data.PromptTotalDurationMoveDown.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCountFrequencyDuration(avgSetTotDur, data.PromptTotalDurationMoveDown.BarCondition, data.PromptTotalDurationMoveDown.TotalTrial, false);
                            if (AccCount < data.PromptTotalDurationMoveDown.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                    }
                    if (data.MoveBackPromptPercentAccuracy.TotalTrial > 0)
                    {
                        bPromptMoveBack = true;
                        //--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(Start)-- //
                        if (data.MoveBackPromptPercentAccuracy.ConsecutiveAverageFailure)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.MoveBackPromptPercentAccuracy.BarCondition, false, data.MoveBackPromptPercentAccuracy.TotalTrial);
                            if (AccConsecutiveCount < data.MoveBackPromptPercentAccuracy.FailureNeeded)
                                bPromptMoveBack = false;
                        }//--- [New Criteria] May 2020 --|MoveBackPromptPercentAccuracy|--(End)-- //
                        else if (data.MoveBackPromptPercentAccuracy.ConsecutiveFailures)
                        {
                            int AccConsecutiveCount = ConsecutiveCount(avgAccSet, data.MoveBackPromptPercentAccuracy.BarCondition, false);
                            if (AccConsecutiveCount < data.MoveBackPromptPercentAccuracy.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                        else
                        {
                            int AccCount = SuccessORFailureCount(avgAccSet, data.MoveBackPromptPercentAccuracy.BarCondition, data.MoveBackPromptPercentAccuracy.TotalTrial, false);
                            if (AccCount < data.MoveBackPromptPercentAccuracy.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                    }
                    if (data.MoveBackPromptPercentIndependence.TotalTrial > 0)
                    {
                        bPromptMoveBack = true;
                        //--- [New Criteria] May 2020 --|MoveBackPromptPercentIndependence|--(Start)-- //
                        if (data.MoveBackPromptPercentIndependence.ConsecutiveAverageFailure)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.MoveBackPromptPercentIndependence.BarCondition, false, data.MoveBackPromptPercentIndependence.TotalTrial);
                            if (AccConsecutiveCount < data.MoveBackPromptPercentIndependence.FailureNeeded)
                                bPromptMoveBack = false;
                        }//--- [New Criteria] May 2020 --|MoveBackPromptPercentIndependence|--(End)-- //
                        else if (data.MoveBackPromptPercentIndependence.ConsecutiveFailures)
                        {
                            int IndConsecutiveCount = ConsecutiveCount(avgAccSet, data.MoveBackPromptPercentIndependence.BarCondition, false);
                            if (IndConsecutiveCount < data.MoveBackPromptPercentIndependence.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                        else
                        {
                            int AIndCount = SuccessORFailureCount(avgAccSet, data.MoveBackPromptPercentIndependence.BarCondition, data.MoveBackPromptPercentIndependence.TotalTrial, false);
                            if (AIndCount < data.MoveBackPromptPercentIndependence.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                    }
                    if (data.PromptTotalIncorrectMoveBack.TotalTrial > 0)
                    {
                        bPromptMoveBack = true;
                        //--- [New Criteria] May 2020 --|PromptTotalIncorrectMoveBack|--(Start)-- //
                        if (data.PromptTotalIncorrectMoveBack.ConsecutiveAverageFailure)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewCorrect(avgIncorrect, data.PromptTotalIncorrectMoveBack.BarCondition, false, data.PromptTotalIncorrectMoveBack.TotalTrial);
                            if (AccConsecutiveCount < data.PromptTotalIncorrectMoveBack.FailureNeeded)
                                bPromptMoveBack = false;
                        }//--- [New Criteria] May 2020 --|PromptTotalIncorrectMoveBack|--(End)-- //
                        else if (data.PromptTotalIncorrectMoveBack.ConsecutiveFailures)
                        {
                            int TIConsecutiveCount = ConsecutiveCountCorrect(avgIncorrect, data.PromptTotalIncorrectMoveBack.BarCondition, false);
                            if (TIConsecutiveCount < data.PromptTotalIncorrectMoveBack.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                        else
                        {
                            int TICount = SuccessORFailureCountCorrect(avgIncorrect, data.PromptTotalIncorrectMoveBack.BarCondition, data.PromptTotalIncorrectMoveBack.TotalTrial, false);
                            if (TICount < data.PromptTotalIncorrectMoveBack.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                    }
                    if (data.PromptTotalCorrectMoveBack.TotalTrial > 0)
                    {
                        bPromptMoveBack = true;
                        //--- [New Criteria] May 2020 --|PromptTotalCorrectMoveBack|--(Start)-- //
                        if (data.PromptTotalCorrectMoveBack.ConsecutiveAverageFailure)
                        {
                            int AccConsecutiveCount = ConsecutiveCountTestNewCorrect(avgCorrect, data.PromptTotalCorrectMoveBack.BarCondition, false, data.PromptTotalCorrectMoveBack.TotalTrial);
                            if (AccConsecutiveCount < data.PromptTotalCorrectMoveBack.FailureNeeded)
                                bPromptMoveBack = false;
                        }//--- [New Criteria] May 2020 --|PromptTotalCorrectMoveBack|--(End)-- //
                        else if (data.PromptTotalCorrectMoveBack.ConsecutiveFailures)
                        {
                            int TIConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, data.PromptTotalCorrectMoveBack.BarCondition, false);
                            if (TIConsecutiveCount < data.PromptTotalCorrectMoveBack.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                        else
                        {
                            int TICount = SuccessORFailureCountCorrect(avgCorrect, data.PromptTotalCorrectMoveBack.BarCondition, data.PromptTotalCorrectMoveBack.TotalTrial, false);
                            if (TICount < data.PromptTotalCorrectMoveBack.FailureNeeded)
                                bPromptMoveBack = false;
                        }
                    }


                    if (bPromptMoveBack)
                    {
                        if (!bpromptColumn)
                        {
                            if (data.PromptHirecharchy)
                            {
                                if (data.NoPromptsUsed.Length > 0)
                                {
                                    int iCurrentIndex = PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt);
                                    if (iCurrentIndex > 0)
                                    {
                                        iCurrentIndex--;
                                        res.NextPrompt = data.NoPromptsUsed[iCurrentIndex];
                                        res.MovedBackPrompt = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            int iCurrentIndex = PromptIndex(data.PromptsUsed, data.CurrentPrompt);
                            if (iCurrentIndex > 0)
                            {
                                iCurrentIndex--;
                                res.NextPrompt = data.PromptsUsed[iCurrentIndex];
                                res.MovedBackPrompt = true;
                            }

                        }
                    }
                }
            }

            //Compare with Target Prompt

            avgAccSet = new float[data.Sessions.Length];
            avgIndSet = new float[data.Sessions.Length];

            avgSetFre = new float[data.Sessions.Length];
            avgSetFreNew = new float[data.Sessions.Length];
            avgSetAvgDur = new float[data.Sessions.Length];
            avgSetTotDur = new float[data.Sessions.Length];
            avgSetAvgDurNew = new float[data.Sessions.Length];
            avgSetTotDurNew = new float[data.Sessions.Length];

            for (int k = 0; k < data.Sessions.Length; k++)
            {
                float AccCount = 0;
                float IndCount = 0;

                float FreCount = 0;
                float AvgDurCount = 0;
                float TotDurCount = 0;
                float FreqScoreSum = 0;
                int FreqScoreSumCount = 0;
                double AvgDurScoreSum = 0;
                int AvgDurScoreSumCount = 0;
                double TotDurScoreSum = 0;
                int TotDurScoreSumCount = 0;

                for (int j = 0; j < data.Sessions[k].Steps.Length; j++)
                {
                    if (data.Sessions[k].Steps[j].Prompt != null)
                    {
                        if (PromptIndex(data.PromptsUsed, data.Sessions[k].Steps[j].Prompt) >= PromptIndex(data.PromptsUsed, data.TargetPrompt))
                        {
                            AccCount++;
                        }
                        if (data.PromptsUsed.Length != 0)
                        {
                            if (data.Sessions[k].Steps[j].Prompt == data.PromptsUsed[data.PromptsUsed.Length - 1])
                            {
                                IndCount++;
                            }
                        }

                        if (data.FrequencyMoveUp.TotalTrial > 0)
                        {
                            int GetStat = data.FreDurMoveStat;
                            string getSetScore = "";

                            if (data.Sessions[k].Steps[j].Prompt != "-1")
                            {
                                FreqScoreSum = FreqScoreSum + float.Parse(data.Sessions[k].Steps[j].Prompt);
                                FreqScoreSumCount++;
                            }

                            if (j == ((data.Sessions[k].Steps.Length) - 1))
                            {
                                if (GetStat == 1)
                                {
                                    if (Convert.ToInt32(FreqScoreSum) < data.FrequencyMoveUp.BarCondition)
                                    {
                                        getSetScore = "+";
                    }
                                    else
                                    {
                                        getSetScore = "-";
                                    }
                                }
                                else if (GetStat == 0)
                                {
                                    if (Convert.ToInt32(FreqScoreSum) > data.FrequencyMoveUp.BarCondition)
                                    {
                                        getSetScore = "+";
                                    }
                                    else
                                    {
                                        getSetScore = "-";
                                    }
                                }
                            }

                            if (PromptIndex(data.PromptsUsed, getSetScore) >= PromptIndex(data.PromptsUsed, data.TargetPrompt))
                            {
                                FreCount++;
                }
                        }
                        if (data.AvgDurationMoveUp.TotalTrial > 0)
                        {
                            int GetStat = data.FreDurMoveStat;
                            string getSetScore = "";

                            if (data.Sessions[k].Steps[j].Prompt != "-1" && data.Sessions[k].Steps[j].Prompt != "00:00:00")
                            {
                                AvgDurScoreSum = AvgDurScoreSum + TimeSpan.Parse(data.Sessions[k].Steps[j].Prompt).TotalSeconds;
                                AvgDurScoreSumCount++;
                            }

                            if (j == ((data.Sessions[k].Steps.Length) - 1))
                            {
                                if (AvgDurScoreSumCount > 0)
                                {
                                    AvgDurScoreSum = AvgDurScoreSum / AvgDurScoreSumCount;
                                }
                                if (GetStat == 1)
                                {
                                    if (Convert.ToInt32(AvgDurScoreSum) < data.AvgDurationMoveUp.BarCondition)
                                    {
                                        getSetScore = "+";
                                    }
                                    else
                                    {
                                        getSetScore = "-";
                                    }
                                }
                                else if (GetStat == 0)
                                {
                                    if (Convert.ToInt32(AvgDurScoreSum) > data.AvgDurationMoveUp.BarCondition)
                                    {
                                        getSetScore = "+";
                                    }
                                    else
                                    {
                                        getSetScore = "-";
                                    }
                                }
                            }

                            if (PromptIndex(data.PromptsUsed, getSetScore) >= PromptIndex(data.PromptsUsed, data.TargetPrompt))
                            {
                                AvgDurCount++;
                            }
                        }

                        if (data.TotalDurationMoveUp.TotalTrial > 0)
                        {
                            int GetStat = data.FreDurMoveStat;
                            string getSetScore = "";

                            if (data.Sessions[k].Steps[j].Prompt != "-1" && data.Sessions[k].Steps[j].Prompt != "00:00:00")
                            {
                                TotDurScoreSum = TotDurScoreSum + TimeSpan.Parse(data.Sessions[k].Steps[j].Prompt).TotalSeconds;
                                TotDurScoreSumCount++;
                            }

                            if (j == ((data.Sessions[k].Steps.Length) - 1))
                            {
                                if (GetStat == 1)
                                {
                                    if (Convert.ToInt32(TotDurScoreSum) < data.TotalDurationMoveUp.BarCondition)
                                    {
                                        getSetScore = "+";
                                    }
                                    else
                                    {
                                        getSetScore = "-";
                                    }
                                }
                                else if (GetStat == 0)
                                {
                                    if (Convert.ToInt32(TotDurScoreSum) > data.TotalDurationMoveUp.BarCondition)
                                    {
                                        getSetScore = "+";
                                    }
                                    else
                                    {
                                        getSetScore = "-";
                                    }
                                }
                            }

                            if (PromptIndex(data.PromptsUsed, getSetScore) >= PromptIndex(data.PromptsUsed, data.TargetPrompt))
                            {
                                TotDurCount++;
                            }
                        } 
                    }

                }
                avgAccSet[k] = AccCount / data.Sessions[k].GetNoScoredSessionCount();
                avgIndSet[k] = IndCount / data.Sessions[k].GetNoScoredSessionCount();

                avgSetFre[k] = (FreCount * FreqScoreSumCount) / data.Sessions[k].GetNoScoredFreqDurSessionCount();

                avgSetAvgDur[k] = (AvgDurCount * AvgDurScoreSumCount) / data.Sessions[k].GetNoScoredFreqDurSessionCount();
                avgSetTotDur[k] = (TotDurCount * TotDurScoreSumCount) / data.Sessions[k].GetNoScoredFreqDurSessionCount();

                float freqscore=(float)FreqScoreSum;
                avgSetFreNew[k] = freqscore;
                float avgdur = (float)AvgDurScoreSum;
                avgSetAvgDurNew[k] = avgdur;
                avgSetTotDur[k] = (TotDurCount * TotDurScoreSumCount) / data.Sessions[k].GetNoScoredFreqDurSessionCount();
                float totDur = (float)TotDurScoreSum;
                avgSetTotDurNew[k] = totDur;
            }


            //if (((data.PercentAccuracy.BarCondition > 0) && (data.PercentAccuracy.BarCondition <= 100)) || ((data.PromptPercentAccuracy.BarCondition == 0)))
            {
                bool bSetSuccess = false;

                //if((data.PercentAccuracy.TotalTrial > 0) || (data.PercentIndependence.TotalTrial > 0))  old code
                if (!bpromptColumn)
                {
                    if ((data.NoPromptsUsed.Length > 0) && (data.promptUp == 0)) // prompt criteria is NA
                    {
                        if ((bStepLevelPrompt || (!bStepLevelPrompt && data.sCurrentLessonPrompt == data.NoPromptsUsed[data.NoPromptsUsed.Length - 1]))
                            && ((data.PercentAccuracy.TotalTrial > 0) || (data.PercentIndependence.TotalTrial > 0) || (data.SetTotalCorrectMoveUp.TotalTrial > 0) || (data.FrequencyMoveUp.TotalTrial > 0) || (data.AvgDurationMoveUp.TotalTrial > 0) || (data.TotalDurationMoveUp.TotalTrial > 0)))
                        {
                            bSetSuccess = true;

                            if (data.FrequencyMoveUp.TotalTrial > 0)
                            {
                                if (data.FrequencyMoveUp.ConsecutiveAverage)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetFreNew, data.FrequencyMoveUp.BarCondition, true, data.FrequencyMoveUp.TotalTrial, data.FreDurMoveStat);
                                    if (AccConsecutiveCount < data.FrequencyMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                                else if (data.FrequencyMoveUp.ConsecutiveSuccess)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetFre, data.FrequencyMoveUp.BarCondition, true);
                                    if (AccConsecutiveCount < data.FrequencyMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                                else
                                {
                                    int AccCount = SuccessORFailureCountFrequencyDuration(avgSetFre, data.FrequencyMoveUp.BarCondition, data.FrequencyMoveUp.TotalTrial, true);
                                    if (AccCount < data.FrequencyMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                            }
                            if (data.AvgDurationMoveUp.TotalTrial > 0)
                            {
                                if (data.AvgDurationMoveUp.ConsecutiveAverage)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetAvgDurNew, data.AvgDurationMoveUp.BarCondition, true, data.AvgDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                                    if (AccConsecutiveCount < data.AvgDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                                else if (data.AvgDurationMoveUp.ConsecutiveSuccess)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetAvgDur, data.AvgDurationMoveUp.BarCondition, true);
                                    if (AccConsecutiveCount < data.AvgDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                                else
                                {
                                    int AccCount = SuccessORFailureCountFrequencyDuration(avgSetAvgDur, data.AvgDurationMoveUp.BarCondition, data.AvgDurationMoveUp.TotalTrial, true);
                                    if (AccCount < data.AvgDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                            }
                            if (data.TotalDurationMoveUp.TotalTrial > 0)
                            {
                                if (data.TotalDurationMoveUp.ConsecutiveAverage)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetTotDurNew, data.TotalDurationMoveUp.BarCondition, true, data.TotalDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                                    if (AccConsecutiveCount < data.TotalDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                                else if (data.TotalDurationMoveUp.ConsecutiveSuccess)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetTotDur, data.TotalDurationMoveUp.BarCondition, true);
                                    if (AccConsecutiveCount < data.TotalDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                                else
                                {
                                    int AccCount = SuccessORFailureCountFrequencyDuration(avgSetTotDur, data.TotalDurationMoveUp.BarCondition, data.TotalDurationMoveUp.TotalTrial, true);
                                    if (AccCount < data.TotalDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                            }

                            if (data.PercentAccuracy.TotalTrial > 0)
                            {
                                //if (!bStepLevelPrompt)
                                {
                                    //--- [New Criteria] May 2020 --|PercentAccuracy|--(Start)-- //
                                    if (data.PercentAccuracy.ConsecutiveAverage)
                                    {
                                        int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.PercentAccuracy.BarCondition, true, data.PercentAccuracy.TotalTrial);
                                        if (AccConsecutiveCount < data.PercentAccuracy.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                                    else if (data.PercentAccuracy.ConsecutiveSuccess)
                                    {
                                        int AccConsecutiveCount = ConsecutiveCount(avgAccSet, data.PercentAccuracy.BarCondition, true);
                                        if (AccConsecutiveCount < data.PercentAccuracy.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }
                                    else
                                    {
                                        int AccCount = SuccessORFailureCount(avgAccSet, data.PercentAccuracy.BarCondition, data.PercentAccuracy.TotalTrial, true);
                                        if (AccCount < data.PercentAccuracy.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }
                                }
                                //else                                
                                //{
                                //bSetSuccess = false;
                                //if (data.PercentAccuracy.ConsecutiveSuccess)
                                //{
                                //    int AccConsecutiveCount = ConsecutiveCount(indPromptSuccess, 1, true);
                                //    if (AccConsecutiveCount < data.PercentAccuracy.SuccessNeeded)
                                //    {
                                //        bSetSuccess = false;
                                //    }
                                //}
                                //else
                                //{
                                //    int AccCount = SuccessORFailureCount(indPromptSuccess, 1, data.PercentAccuracy.TotalTrial, true);
                                //    if (AccCount < data.PercentAccuracy.SuccessNeeded)
                                //    {
                                //        bSetSuccess = false;
                                //    }
                                //}
                                //}
                            }


                            if (data.PercentIndependence.TotalTrial > 0)
                            {
                                //if (!bStepLevelPrompt)
                                {
                                    //--- [New Criteria] May 2020 --|PercentIndependence|--(Start)-- //
                                    if (data.PercentIndependence.ConsecutiveAverage)
                                    {
                                        int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.PercentIndependence.BarCondition, true, data.PercentIndependence.TotalTrial);
                                        if (AccConsecutiveCount < data.PercentIndependence.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }//--- [New Criteria] May 2020 --|PercentIndependence|--(End)-- //
                                    else if (data.PercentIndependence.ConsecutiveSuccess)
                                    {
                                        int AccConsecutiveCount = ConsecutiveCount(avgIndSet, data.PercentIndependence.BarCondition, true);
                                        if (AccConsecutiveCount < data.PercentIndependence.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }
                                    else
                                    {
                                        int AccCount = SuccessORFailureCount(avgIndSet, data.PercentIndependence.BarCondition, data.PercentIndependence.TotalTrial, true);
                                        if (AccCount < data.PercentIndependence.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }
                                }
                                //else
                                //{
                                //    bSetSuccess = false;
                                //    if (data.PercentIndependence.ConsecutiveSuccess)
                                //    {
                                //        int AccConsecutiveCount = ConsecutiveCount(indPromptSuccess, 1, true);
                                //        if (AccConsecutiveCount < data.PercentIndependence.SuccessNeeded)
                                //        {
                                //            bSetSuccess = false;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        int AccCount = SuccessORFailureCount(indPromptSuccess, 1, data.PercentIndependence.TotalTrial, true);
                                //        if (AccCount < data.PercentIndependence.SuccessNeeded)
                                //        {
                                //            bSetSuccess = false;
                                //        }
                                //    }
                                //}
                            }
                            if (data.SetTotalCorrectMoveUp.TotalTrial > 0)
                            {
                                //--- [New Criteria] May 2020 --|SetTotalCorrectMoveUp|--(Start)-- //
                                if (data.SetTotalCorrectMoveUp.ConsecutiveAverage)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNewCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, true, data.SetTotalCorrectMoveUp.TotalTrial);
                                    if (AccConsecutiveCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }//--- [New Criteria] May 2020 --|SetTotalCorrectMoveUp|--(End)-- //
                                else if (data.SetTotalCorrectMoveUp.ConsecutiveSuccess)
                                {
                                    int TCConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, true);
                                    if (TCConsecutiveCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                                else
                                {
                                    int TCCount = SuccessORFailureCountCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, data.SetTotalCorrectMoveUp.TotalTrial, true);
                                    if (TCCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((bStepLevelPrompt || (!bStepLevelPrompt && data.CurrentPrompt == data.TargetPrompt))
                       && ((data.PercentAccuracy.TotalTrial > 0) || (data.PercentIndependence.TotalTrial > 0) || (data.SetTotalCorrectMoveUp.TotalTrial > 0) || (data.FrequencyMoveUp.TotalTrial > 0) || (data.AvgDurationMoveUp.TotalTrial > 0) || (data.TotalDurationMoveUp.TotalTrial > 0)))
                        {
                            bSetSuccess = true;

                            if (data.FrequencyMoveUp.TotalTrial > 0)
                            {                                
                                //--- [New Criteria] May 2020 --|PercentAccuracy|--(Start)-- //
                                if (data.FrequencyMoveUp.ConsecutiveAverage)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetFreNew, data.FrequencyMoveUp.BarCondition, true, data.FrequencyMoveUp.TotalTrial, data.FreDurMoveStat);
                                    if (AccConsecutiveCount < data.FrequencyMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                                else if (data.FrequencyMoveUp.ConsecutiveSuccess)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetFre, data.FrequencyMoveUp.BarCondition, true);
                                    if (AccConsecutiveCount < data.FrequencyMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                                else
                                {
                                    int AccCount = SuccessORFailureCountFrequencyDuration(avgSetFre, data.FrequencyMoveUp.BarCondition, data.FrequencyMoveUp.TotalTrial, true);
                                    if (AccCount < data.FrequencyMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                            }
                            if (data.AvgDurationMoveUp.TotalTrial > 0)
                            {
                                if (data.AvgDurationMoveUp.ConsecutiveAverage)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetAvgDurNew, data.AvgDurationMoveUp.BarCondition, true, data.AvgDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                                    if (AccConsecutiveCount < data.AvgDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                                else if (data.AvgDurationMoveUp.ConsecutiveSuccess)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetAvgDur, data.AvgDurationMoveUp.BarCondition, true);
                                    if (AccConsecutiveCount < data.AvgDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                                else
                                {
                                    int AccCount = SuccessORFailureCountFrequencyDuration(avgSetAvgDur, data.AvgDurationMoveUp.BarCondition, data.AvgDurationMoveUp.TotalTrial, true);
                                    if (AccCount < data.AvgDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                            }
                            if (data.TotalDurationMoveUp.TotalTrial > 0)
                            {
                                if (data.TotalDurationMoveUp.ConsecutiveAverage)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetTotDurNew, data.TotalDurationMoveUp.BarCondition, true, data.TotalDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                                    if (AccConsecutiveCount < data.TotalDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                                else if (data.TotalDurationMoveUp.ConsecutiveSuccess)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetTotDur, data.TotalDurationMoveUp.BarCondition, true);
                                    if (AccConsecutiveCount < data.TotalDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                                else
                                {
                                    int AccCount = SuccessORFailureCountFrequencyDuration(avgSetTotDur, data.TotalDurationMoveUp.BarCondition, data.TotalDurationMoveUp.TotalTrial, true);
                                    if (AccCount < data.TotalDurationMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                            }

                            if (data.PercentAccuracy.TotalTrial > 0)
                            {
                                //if (!bStepLevelPrompt)
                                {
                                    //--- [New Criteria] May 2020 --|PercentAccuracy|--(Start)-- //
                                    if (data.PercentAccuracy.ConsecutiveAverage)
                                    {
                                        int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.PercentAccuracy.BarCondition, true, data.PercentAccuracy.TotalTrial);
                                        if (AccConsecutiveCount < data.PercentAccuracy.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                                    else if (data.PercentAccuracy.ConsecutiveSuccess)
                                    {
                                        int AccConsecutiveCount = ConsecutiveCount(avgAccSet, data.PercentAccuracy.BarCondition, true);
                                        if (AccConsecutiveCount < data.PercentAccuracy.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }
                                    else
                                    {
                                        int AccCount = SuccessORFailureCount(avgAccSet, data.PercentAccuracy.BarCondition, data.PercentAccuracy.TotalTrial, true);
                                        if (AccCount < data.PercentAccuracy.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }
                                }

                            }

                            if (data.PercentIndependence.TotalTrial > 0)
                            {
                                //if (!bStepLevelPrompt)
                                {
                                    //--- [New Criteria] May 2020 --|PercentIndependence|--(Start)-- //
                                    if (data.PercentIndependence.ConsecutiveAverage)
                                    {
                                        int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.PercentIndependence.BarCondition, true, data.PercentIndependence.TotalTrial);
                                        if (AccConsecutiveCount < data.PercentIndependence.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }//--- [New Criteria] May 2020 --|PercentIndependence|--(End)-- //
                                    else if (data.PercentIndependence.ConsecutiveSuccess)
                                    {
                                        int AccConsecutiveCount = ConsecutiveCount(avgIndSet, data.PercentIndependence.BarCondition, true);
                                        if (AccConsecutiveCount < data.PercentIndependence.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }
                                    else
                                    {
                                        int AccCount = SuccessORFailureCount(avgIndSet, data.PercentIndependence.BarCondition, data.PercentIndependence.TotalTrial, true);
                                        if (AccCount < data.PercentIndependence.SuccessNeeded)
                                        {
                                            bSetSuccess = false;
                                        }
                                    }
                                }

                            }
                            if (data.SetTotalCorrectMoveUp.TotalTrial > 0)
                            {
                                //--- [New Criteria] May 2020 --|SetTotalCorrectMoveUp|--(Start)-- //
                                if (data.SetTotalCorrectMoveUp.ConsecutiveAverage)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNewCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, true,data.SetTotalCorrectMoveUp.TotalTrial);
                                    if (AccConsecutiveCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }//--- [New Criteria] May 2020 --|SetTotalCorrectMoveUp|--(End)-- //
                                else if (data.SetTotalCorrectMoveUp.ConsecutiveSuccess)
                                {
                                    int TCConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, true);
                                    if (TCConsecutiveCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                                else
                                {
                                    int TCCount = SuccessORFailureCountCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, data.SetTotalCorrectMoveUp.TotalTrial, true);
                                    if (TCCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                            }
                        }
                    }

                }
                else
                {
                    if ((bStepLevelPrompt || (!bStepLevelPrompt && data.CurrentPrompt == data.TargetPrompt))
                        && ((data.PercentAccuracy.TotalTrial > 0) || (data.PercentIndependence.TotalTrial > 0) || (data.SetTotalCorrectMoveUp.TotalTrial > 0) || (data.FrequencyMoveUp.TotalTrial > 0) || (data.AvgDurationMoveUp.TotalTrial > 0) || (data.TotalDurationMoveUp.TotalTrial > 0)))
                    {
                        bSetSuccess = true;

                        if (data.FrequencyMoveUp.TotalTrial > 0)
                        {
                            //--- [New Criteria] May 2020 --|PercentAccuracy|--(Start)-- //
                            if (data.FrequencyMoveUp.ConsecutiveAverage)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetFreNew, data.FrequencyMoveUp.BarCondition, true, data.FrequencyMoveUp.TotalTrial, data.FreDurMoveStat);
                                if (AccConsecutiveCount < data.FrequencyMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                            else if (data.PercentAccuracy.ConsecutiveSuccess)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetFre, data.FrequencyMoveUp.BarCondition, true);
                                if (AccConsecutiveCount < data.FrequencyMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgSetFre, data.FrequencyMoveUp.BarCondition, data.FrequencyMoveUp.TotalTrial, true);
                                if (AccCount < data.FrequencyMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }
                        }
                        if (data.AvgDurationMoveUp.TotalTrial > 0)
                        {
                            if (data.AvgDurationMoveUp.ConsecutiveAverage)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetAvgDurNew, data.AvgDurationMoveUp.BarCondition, true, data.AvgDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                                if (AccConsecutiveCount < data.AvgDurationMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                            else if (data.AvgDurationMoveUp.ConsecutiveSuccess)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetAvgDur, data.AvgDurationMoveUp.BarCondition, true);
                                if (AccConsecutiveCount < data.AvgDurationMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgSetAvgDur, data.AvgDurationMoveUp.BarCondition, data.AvgDurationMoveUp.TotalTrial, true);
                                if (AccCount < data.AvgDurationMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }
                        }
                        if (data.TotalDurationMoveUp.TotalTrial > 0)
                        {
                            if (data.TotalDurationMoveUp.ConsecutiveAverage)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetTotDurNew, data.TotalDurationMoveUp.BarCondition, true, data.TotalDurationMoveUp.TotalTrial, data.FreDurMoveStat);
                                if (AccConsecutiveCount < data.TotalDurationMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                            else if (data.TotalDurationMoveUp.ConsecutiveSuccess)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetTotDur, data.TotalDurationMoveUp.BarCondition, true);
                                if (AccConsecutiveCount < data.TotalDurationMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgSetTotDur, data.TotalDurationMoveUp.BarCondition, data.TotalDurationMoveUp.TotalTrial, true);
                                if (AccCount < data.TotalDurationMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }
                        }


                        if (data.PercentAccuracy.TotalTrial > 0)
                        {
                            //if (!bStepLevelPrompt)
                            {
                                //--- [New Criteria] May 2020 --|PercentAccuracy|--(Start)-- //
                                if (data.PercentAccuracy.ConsecutiveAverage)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.PercentAccuracy.BarCondition, true, data.PercentAccuracy.TotalTrial);
                                    if (AccConsecutiveCount < data.PercentAccuracy.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }//--- [New Criteria] May 2020 --|PercentAccuracy|--(End)-- //
                                else if (data.PercentAccuracy.ConsecutiveSuccess)
                                {
                                    int AccConsecutiveCount = ConsecutiveCount(avgAccSet, data.PercentAccuracy.BarCondition, true);
                                    if (AccConsecutiveCount < data.PercentAccuracy.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                                else
                                {
                                    int AccCount = SuccessORFailureCount(avgAccSet, data.PercentAccuracy.BarCondition, data.PercentAccuracy.TotalTrial, true);
                                    if (AccCount < data.PercentAccuracy.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                            }
                            //else                            
                            //{
                            //bSetSuccess = false;
                            //if (data.PercentAccuracy.ConsecutiveSuccess)
                            //{
                            //    int AccConsecutiveCount = ConsecutiveCount(indPromptSuccess, 1, true);
                            //    if (AccConsecutiveCount < data.PercentAccuracy.SuccessNeeded)
                            //    {
                            //        bSetSuccess = false;
                            //    }
                            //}
                            //else
                            //{
                            //    int AccCount = SuccessORFailureCount(indPromptSuccess, 1, data.PercentAccuracy.TotalTrial, true);
                            //    if (AccCount < data.PercentAccuracy.SuccessNeeded)
                            //    {
                            //        bSetSuccess = false;
                            //    }
                            //}
                            //}
                        }

                        if (data.PercentIndependence.TotalTrial > 0)
                        {
                            //if (!bStepLevelPrompt)
                            {
                                //--- [New Criteria] May 2020 --|PercentIndependence|--(Start)-- //
                                if (data.PercentIndependence.ConsecutiveAverage)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.PercentIndependence.BarCondition, true, data.PercentIndependence.TotalTrial);
                                    if (AccConsecutiveCount < data.PercentIndependence.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }//--- [New Criteria] May 2020 --|PercentIndependence|--(End)-- //
                                else if (data.PercentIndependence.ConsecutiveSuccess)
                                {
                                    int AccConsecutiveCount = ConsecutiveCount(avgIndSet, data.PercentIndependence.BarCondition, true);
                                    if (AccConsecutiveCount < data.PercentIndependence.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                                else
                                {
                                    int AccCount = SuccessORFailureCount(avgIndSet, data.PercentIndependence.BarCondition, data.PercentIndependence.TotalTrial, true);
                                    if (AccCount < data.PercentIndependence.SuccessNeeded)
                                    {
                                        bSetSuccess = false;
                                    }
                                }
                            }
                            //else
                            //{
                            //    bSetSuccess = false;
                            //    if (data.PercentIndependence.ConsecutiveSuccess)
                            //    {
                            //        int AccConsecutiveCount = ConsecutiveCount(indPromptSuccess, 1, true);
                            //        if (AccConsecutiveCount < data.PercentIndependence.SuccessNeeded)
                            //        {
                            //            bSetSuccess = false;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        int AccCount = SuccessORFailureCount(indPromptSuccess, 1, data.PercentIndependence.TotalTrial, true);
                            //        if (AccCount < data.PercentIndependence.SuccessNeeded)
                            //        {
                            //            bSetSuccess = false;
                            //        }
                            //    }
                            //}
                        }
                        if (data.SetTotalCorrectMoveUp.TotalTrial > 0)
                        {
                            //--- [New Criteria] May 2020 --|SetTotalCorrectMoveUp|--(Start)-- //
                            if (data.SetTotalCorrectMoveUp.ConsecutiveAverage)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNewCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, true,data.SetTotalCorrectMoveUp.TotalTrial);
                                if (AccConsecutiveCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }//--- [New Criteria] May 2020 --|SetTotalCorrectMoveUp|--(End)-- //
                            else if (data.SetTotalCorrectMoveUp.ConsecutiveSuccess)
                            {
                                int TCConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, true);
                                if (TCConsecutiveCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }
                            else
                            {
                                int TCCount = SuccessORFailureCountCorrect(avgCorrect, data.SetTotalCorrectMoveUp.BarCondition, data.SetTotalCorrectMoveUp.TotalTrial, true);
                                if (TCCount < data.SetTotalCorrectMoveUp.SuccessNeeded)
                                {
                                    bSetSuccess = false;
                                }
                            }
                        }
                    }
                }
                if (bSetSuccess)
                {
                    if (data.CurrentSet == data.TotalSets)
                    {
                        res.CompletionStatus = "COMPLETED";
                        res.MovedForwardSet = true;
                        res.NextPrompt = "0";
                        res.NextSet = data.CurrentSet;
                    }
                    else
                    {
                        res.NextSet = data.CurrentSet + 1;
                        //res.NextPrompt = data.PromptsUsed[0];
                        res.MovedForwardSet = true;
                    }
                }
                else
                {
                    if (!bpromptColumn)// Check if the colum is prompt.
                    {
                        if ((data.NoPromptsUsed.Length > 0) && (data.promptDown == 0)) // prompt criteria is NA
                        {
                            if (PromptIndex(data.NoPromptsUsed, data.sCurrentLessonPrompt) > PromptIndex(data.NoPromptsUsed, data.NoPromptsUsed[0]))
                            {
                                return res;
                            }
                        }
                    }

                    bool bMoveBack = false;
                    if (data.MoveBackPercentAccuracy.TotalTrial > 0 || data.MoveBackPercentIndependence.TotalTrial > 0 || data.SetTotalIncorrectMoveBack.TotalTrial > 0 || data.SetTotalCorrectMoveBack.TotalTrial > 0 || data.FrequencyMoveDown.TotalTrial > 0 || data.AvgDurationMoveDown.TotalTrial > 0 || data.TotalDurationMoveDown.TotalTrial > 0)
                    {
                        bMoveBack = true;
                        //Check for Set Move Back

                        if (data.FrequencyMoveDown.TotalTrial > 0)
                        { 
                            //--- [New Criteria] May 2020 --|MoveBackPercentAccuracy|--(Start)-- //
                            if (data.FrequencyMoveDown.ConsecutiveAverageFailure)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetFreNew, data.FrequencyMoveDown.BarCondition, false, data.FrequencyMoveDown.TotalTrial, data.FreDurMoveStat);
                                if (AccConsecutiveCount < data.FrequencyMoveDown.FailureNeeded)
                                    bMoveBack = false;
                            }//--- [New Criteria] May 2020 --|MoveBackPercentAccuracy|--(End)-- //
                            else if (data.FrequencyMoveDown.ConsecutiveFailures)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetFre, data.FrequencyMoveDown.BarCondition, false);
                                if (AccConsecutiveCount < data.FrequencyMoveDown.FailureNeeded)
                                    bMoveBack = false;
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgSetFre, data.FrequencyMoveDown.BarCondition, data.FrequencyMoveDown.TotalTrial, false);
                                if (AccCount < data.FrequencyMoveDown.FailureNeeded)
                                    bMoveBack = false;
                            }
                        }
                        if (data.AvgDurationMoveDown.TotalTrial > 0)
                        {
                            //--- [New Criteria] May 2020 --|MoveBackPercentAccuracy|--(Start)-- //
                            if (data.AvgDurationMoveDown.ConsecutiveAverageFailure)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetAvgDurNew, data.AvgDurationMoveDown.BarCondition, false, data.AvgDurationMoveDown.TotalTrial, data.FreDurMoveStat);
                                if (AccConsecutiveCount < data.AvgDurationMoveDown.FailureNeeded)
                                    bMoveBack = false;
                            }//--- [New Criteria] May 2020 --|MoveBackPercentAccuracy|--(End)-- //
                            else if (data.AvgDurationMoveDown.ConsecutiveFailures)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetAvgDur, data.AvgDurationMoveDown.BarCondition, false);
                                if (AccConsecutiveCount < data.AvgDurationMoveDown.FailureNeeded)
                                    bMoveBack = false;
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgSetAvgDur, data.AvgDurationMoveDown.BarCondition, data.AvgDurationMoveDown.TotalTrial, false);
                                if (AccCount < data.AvgDurationMoveDown.FailureNeeded)
                                    bMoveBack = false;
                            }
                        }
                        if (data.TotalDurationMoveDown.TotalTrial > 0)
                        {
                            //--- [New Criteria] May 2020 --|MoveBackPercentAccuracy|--(Start)-- //
                            if (data.TotalDurationMoveDown.ConsecutiveAverageFailure)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNewAvgDur(avgSetTotDurNew, data.TotalDurationMoveDown.BarCondition, false, data.TotalDurationMoveDown.TotalTrial, data.FreDurMoveStat);
                                if (AccConsecutiveCount < data.TotalDurationMoveDown.FailureNeeded)
                                    bMoveBack = false;
                            }//--- [New Criteria] May 2020 --|MoveBackPercentAccuracy|--(End)-- //
                            else if (data.TotalDurationMoveDown.ConsecutiveFailures)
                            {
                                int AccConsecutiveCount = ConsecutiveCountfreqd(avgSetTotDur, data.TotalDurationMoveDown.BarCondition, false);
                                if (AccConsecutiveCount < data.TotalDurationMoveDown.FailureNeeded)
                                    bMoveBack = false;
                            }
                            else
                            {
                                int AccCount = SuccessORFailureCountFrequencyDuration(avgSetTotDur, data.TotalDurationMoveDown.BarCondition, data.TotalDurationMoveDown.TotalTrial, false);
                                if (AccCount < data.TotalDurationMoveDown.FailureNeeded)
                                    bMoveBack = false;
                            }
                        }

                        if (data.MoveBackPercentAccuracy.TotalTrial > 0)
                        {
                            //if (!bStepLevelPrompt)
                            {
                                //--- [New Criteria] May 2020 --|MoveBackPercentAccuracy|--(Start)-- //
                                if (data.MoveBackPercentAccuracy.ConsecutiveAverageFailure)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.MoveBackPercentAccuracy.BarCondition, false, data.MoveBackPercentAccuracy.TotalTrial);
                                    if (AccConsecutiveCount < data.MoveBackPercentAccuracy.FailureNeeded)
                                        bMoveBack = false;
                                }//--- [New Criteria] May 2020 --|MoveBackPercentAccuracy|--(End)-- //
                                else if (data.MoveBackPercentAccuracy.ConsecutiveFailures)
                                {
                                    int AccConsecutiveCount = ConsecutiveCount(avgAccSet, data.MoveBackPercentAccuracy.BarCondition, false);
                                    if (AccConsecutiveCount < data.MoveBackPercentAccuracy.FailureNeeded)
                                        bMoveBack = false;
                                }
                                else
                                {
                                    int AccCount = SuccessORFailureCount(avgAccSet, data.MoveBackPercentAccuracy.BarCondition, data.MoveBackPercentAccuracy.TotalTrial, false);
                                    if (AccCount < data.MoveBackPercentAccuracy.FailureNeeded)
                                        bMoveBack = false;
                                }
                            }
                            //else
                            //{
                            //    //Check the Current Step Prompts array
                            //    if (data.MoveBackPercentAccuracy.ConsecutiveFailures)
                            //    {
                            //        int AccConsecutiveCount = ConsecutiveCount(indPromptSuccess, 0, false);
                            //        if (AccConsecutiveCount < data.MoveBackPercentAccuracy.FailureNeeded)
                            //            bMoveBack = false;
                            //    }
                            //    else
                            //    {
                            //        int AccCount = SuccessORFailureCount(indPromptSuccess, 0, data.MoveBackPercentAccuracy.TotalTrial, false);
                            //        if (AccCount < data.MoveBackPercentAccuracy.FailureNeeded)
                            //            bMoveBack = false;
                            //    }
                            //}
                        }
                        if (data.MoveBackPercentIndependence.TotalTrial > 0)
                        {
                            //if (!bStepLevelPrompt)
                            {
                                //--- [New Criteria] May 2020 --|MoveBackPercentIndependence|--(Start)-- //
                                if (data.MoveBackPercentIndependence.ConsecutiveAverageFailure)
                                {
                                    int AccConsecutiveCount = ConsecutiveCountTestNew(avgAccSet, data.MoveBackPercentIndependence.BarCondition, false, data.MoveBackPercentIndependence.TotalTrial);
                                    if (AccConsecutiveCount < data.MoveBackPercentIndependence.FailureNeeded)
                                    {
                                        bMoveBack = false;
                                    }
                                }//--- [New Criteria] May 2020 --|MoveBackPercentIndependence|--(End)-- //
                                else if (data.MoveBackPercentIndependence.ConsecutiveFailures)
                                {
                                    int AccConsecutiveCount = ConsecutiveCount(avgIndSet, data.MoveBackPercentIndependence.BarCondition, false);
                                    if (AccConsecutiveCount < data.MoveBackPercentIndependence.FailureNeeded)
                                        bMoveBack = false;
                                }
                                else
                                {
                                    int AccCount = SuccessORFailureCount(avgIndSet, data.MoveBackPercentIndependence.BarCondition, data.MoveBackPercentIndependence.TotalTrial, false);
                                    if (AccCount < data.MoveBackPercentIndependence.FailureNeeded)
                                        bMoveBack = false;
                                }
                            }
                            //else
                            //{
                            //    //Check the Current Step Prompts array
                            //    if (data.MoveBackPercentIndependence.ConsecutiveFailures)
                            //    {
                            //        int AccConsecutiveCount = ConsecutiveCount(indPromptSuccess, 0, false);
                            //        if (AccConsecutiveCount < data.MoveBackPercentIndependence.FailureNeeded)
                            //            bMoveBack = false;
                            //    }
                            //    else
                            //    {
                            //        int AccCount = SuccessORFailureCount(indPromptSuccess, 0, data.MoveBackPercentIndependence.TotalTrial, false);
                            //        if (AccCount < data.MoveBackPercentIndependence.FailureNeeded)
                            //            bMoveBack = false;
                            //    }
                            //}
                        }
                        if (data.SetTotalIncorrectMoveBack.TotalTrial > 0)
                        {
                            //--- [New Criteria] May 2020 --|SetTotalIncorrectMoveBack|--(Start)-- //
                            if (data.SetTotalIncorrectMoveBack.ConsecutiveAverageFailure)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNewCorrect(avgIncorrect, data.SetTotalIncorrectMoveBack.BarCondition, false,data.SetTotalIncorrectMoveBack.TotalTrial);
                                if (AccConsecutiveCount < data.SetTotalIncorrectMoveBack.FailureNeeded)
                                {
                                    bMoveBack = false;
                                }
                            }//--- [New Criteria] May 2020 --|SetTotalIncorrectMoveBack|--(End)-- //
                            else if (data.SetTotalIncorrectMoveBack.ConsecutiveFailures)
                            {
                                int TCConsecutiveCount = ConsecutiveCountCorrect(avgIncorrect, data.SetTotalIncorrectMoveBack.BarCondition, false);
                                if (TCConsecutiveCount < data.SetTotalIncorrectMoveBack.FailureNeeded)
                                    bMoveBack = false;
                            }
                            else
                            {
                                int TICount = SuccessORFailureCountCorrect(avgIncorrect, data.SetTotalIncorrectMoveBack.BarCondition, data.SetTotalIncorrectMoveBack.TotalTrial, false);
                                if (TICount < data.SetTotalIncorrectMoveBack.FailureNeeded)
                                    bMoveBack = false;
                            }
                        }
                        if (data.SetTotalCorrectMoveBack.TotalTrial > 0)
                        {
                            //--- [New Criteria] May 2020 --|SetTotalCorrectMoveBack|--(Start)-- //
                            if (data.SetTotalCorrectMoveBack.ConsecutiveAverageFailure)
                            {
                                int AccConsecutiveCount = ConsecutiveCountTestNewCorrect(avgCorrect, data.SetTotalCorrectMoveBack.BarCondition, false,data.SetTotalCorrectMoveBack.TotalTrial);
                                if (AccConsecutiveCount < data.SetTotalCorrectMoveBack.FailureNeeded)
                                {
                                    bMoveBack = false;
                                }
                            }//--- [New Criteria] May 2020 --|SetTotalCorrectMoveBack|--(End)-- //
                            else if (data.SetTotalCorrectMoveBack.ConsecutiveFailures)
                            {
                                int TCConsecutiveCount = ConsecutiveCountCorrect(avgCorrect, data.SetTotalCorrectMoveBack.BarCondition, false);
                                if (TCConsecutiveCount < data.SetTotalCorrectMoveBack.FailureNeeded)
                                    bMoveBack = false;
                            }
                            else
                            {
                                int TICount = SuccessORFailureCountCorrect(avgCorrect, data.SetTotalCorrectMoveBack.BarCondition, data.SetTotalCorrectMoveBack.TotalTrial, false);
                                if (TICount < data.SetTotalCorrectMoveBack.FailureNeeded)
                                    bMoveBack = false;
                            }
                        }
                    }
                    if (bMoveBack && data.CurrentSet > 1)
                    {
                        //res.MoveBackPromptStep = true res.MovedBackPrompt
                        if (bStepLevelPrompt)
                        {
                            if (!res.MoveBackPromptStep)
                            {
                                res.NextSet = data.CurrentSet - 1;
                                res.NextPrompt = data.TargetPrompt;
                                res.MovedBackSet = true;
                            }
                        }
                        else
                        {
                            if (!res.MovedBackPrompt)
                            {
                                res.NextSet = data.CurrentSet - 1;
                                res.NextPrompt = data.TargetPrompt;
                                res.MovedBackSet = true;
                            }
                        }

                    }
                }
            }
            return res;
        }

    }
}

//Is the IOA and multiteacher set at SET level or Lesson Plan level??
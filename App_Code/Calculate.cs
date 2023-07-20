using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Calculate
{
    public enum Operation
    {
        SUM=0,
        AVG=1,
        COUNT=2

    }
   
    public class ColumnData
    {
        public string Formula="";
        public string ColumnName = "";
        public Operation  OperationName;
        public float[] Data = null;
    }

    public class PreProcessedExpression
    {
        public string Expression = "";
        public ColumnData[] ColumnDatas = null;
    }

    public class Calculate
    {
        

        public string CreateExpressionToEvaluate(PreProcessedExpression preExpression)
        {
           

            foreach (ColumnData data in preExpression.ColumnDatas)
            {
                float fResult = 0;

                switch (data.OperationName)
                {
                    case Operation.SUM:
   
                        for (int i = 0; i < data.Data.Length; i++)
                        {
                            fResult += data.Data[i];
                        }
                        preExpression.Expression=preExpression.Expression.Replace(data.Formula, fResult.ToString());
                        break;
                    case Operation.AVG:
 
                        for (int i = 0; i < data.Data.Length; i++)
                        {
                            fResult += data.Data[i];
                        }
                        fResult = fResult / data.Data.Length;
                        preExpression.Expression=preExpression.Expression.Replace(data.Formula, fResult.ToString());
                        break;
                    case Operation.COUNT:

                        preExpression.Expression = preExpression.Expression.Replace(data.Formula, data.Data.Length.ToString());
                        break;
                }
            }

            return preExpression.Expression;
        }
        public PreProcessedExpression PreProcessExpression(string Expression)
        {
            string[] OperationString = new string[] { "SUM", "AVG", "COUNT" };
            ArrayList arrayList = new ArrayList();
            int iFormulaIndex = 0;
            bool bContinue = true;
            Expression=Expression.ToUpper();
            PreProcessedExpression Result = new PreProcessedExpression();
            while (bContinue)
            {
                bContinue = false;
                for (int i = 0; i < OperationString.Length;i++ )
                {
                    int index = Expression.IndexOf(OperationString[i]);
                    if (index >= 0)
                    {
                        bContinue = true;
                        ColumnData data = new ColumnData();
                        data.OperationName = (Operation)i;
                        int startParan = Expression.IndexOf('(', index);
                        int endParan = Expression.IndexOf(')', startParan);
                        data.ColumnName = Expression.Substring(startParan + 1, endParan - startParan - 1);
                        data.Formula = "F" + iFormulaIndex.ToString();
                        Expression=Expression.Replace(Expression.Substring(index,endParan-index+1), " " + data.Formula + " ");
                        arrayList.Add(data);
                    }
                }
            }
            Result.Expression = Expression;
            Result.ColumnDatas = new ColumnData[arrayList.Count];
            for(int i=0;i<arrayList.Count;i++)
            {

                Result.ColumnDatas[i] = (ColumnData)arrayList[i];
            }
            return Result;
        }
        private int Priority(string Operator)
        {
            switch (Operator)
            {
                case "+":
                    return 0;
                case "-":
                    return 1;
                case "/":
                    return 2;
                case "*":
                    return 3;
                default:
                    return -1;
            }
        }
        public string[] InfixToPostfix(string[] infixArray)
        {
            var stack = new Stack<string>();
            var postfix = new Stack<string>();

            string st;
            for (int i = 0; i < infixArray.Length; i++)
            {
                if (!("()*/+-".Contains(infixArray[i])))
                {
                    postfix.Push(infixArray[i]);
                }
                else
                {
                    if (infixArray[i].Equals("("))
                    {
                        stack.Push("(");
                    }
                    else if (infixArray[i].Equals(")"))
                    {
                        st = stack.Pop();
                        while (!(st.Equals("(")))
                        {
                            postfix.Push(st);
                            st = stack.Pop();
                        }
                    }
                    else
                    {
                        while (stack.Count > 0)
                        {
                            st = stack.Pop();
                            if (Priority(st) >= Priority(infixArray[i]))
                            {
                                postfix.Push(st);
                            }
                            else
                            {
                                stack.Push(st);
                                break;
                            }
                        }
                        stack.Push(infixArray[i]);
                    }
                }
            }
            while (stack.Count > 0)
            {
                postfix.Push(stack.Pop());
            }

            return postfix.ToArray();
            //return postfix.Reverse().ToArray();
        }

        public float EvaluatePostfix(string[] postfixArray)
        {

            var stack = new Stack<string>(postfixArray);
            var tempstack = new Stack<string>(postfixArray);

            while (stack.Count != 0)
            {
                string poped = stack.Pop().Trim();

                switch (poped)
                {
                    case "+":
                        if (tempstack.Count >= 2)
                        {
                            float a = float.Parse(tempstack.Pop());
                            float b = float.Parse(tempstack.Pop());
                            float c = a + b;
                            stack.Push(c.ToString());
                        }
                        else
                        {
                            stack.Push(tempstack.Pop());
                        }
                        break;
                    case "-":
                        if (tempstack.Count >= 2)
                        {
                            float a = float.Parse(tempstack.Pop());
                            float b = float.Parse(tempstack.Pop());
                            float c = b - a;
                            stack.Push(c.ToString());
                        }
                        else
                        {
                            float a = float.Parse(tempstack.Pop()) * -1;
                            stack.Push(a.ToString());
                        }
                        break;
                    case "*":
                        if (tempstack.Count >= 2)
                        {
                            float a = float.Parse(tempstack.Pop());
                            float b = float.Parse(tempstack.Pop());
                            float c = a * b;
                            stack.Push(c.ToString());
                        }
                        else
                        {
                            stack.Push(tempstack.Pop());
                        }
                        break;
                    case "/":
                        if (tempstack.Count >= 2)
                        {
                            float a = float.Parse(tempstack.Pop());
                            float b = float.Parse(tempstack.Pop());
                            float c =b/a;
                            stack.Push(c.ToString());
                        }
                        else
                        {
                            stack.Push(tempstack.Pop());
                        }
                        break;
                    default:
                        tempstack.Push(poped);
                        break;
                }
            }

            if (tempstack.Count > 0)
                return float.Parse(tempstack.Pop());
            else
                return -1;
        }
    }
}

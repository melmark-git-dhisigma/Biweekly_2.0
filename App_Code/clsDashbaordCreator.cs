using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class clsDashbaordCreator
{
    public clsDashbaordCreator()
    {

    }
    public void plotLeftAndRightSide(string[] arNames, out string Names, int[] Lar, out string LG, int[] Rar, out string RG, out string Height, string Type)
    {
        int LGLength = Lar.Length;
        int RGLength = Rar.Length;
        int NMLength = arNames.Length;

        int he = 0;

        if (LGLength > RGLength)
        {
            he = LGLength * 50 + 45;
        }
        else if (LGLength == RGLength)
        {
            he = LGLength * 50 + 45;
        }
        else
        {
            he = RGLength * 50 + 45;
        }


        Height = he.ToString() + "px";

        string strCreate = "";
        int width = 0;
        string score = "";
        string wdpx = "";
        string MarginBottom = "";

        for (int i = 0; i < LGLength; i++)
        {
            if (Convert.ToInt16(Lar[i]) == 0)
            {
                width =0;
                wdpx = "0 !important;padding:0 !important ";
                MarginBottom = "35px";
            }
            else
            {
                width = Convert.ToInt16(Lar[i]) * 5 - 5;
                wdpx = width.ToString() + "px";
                MarginBottom = "28px";
            }
            if (width > 500) width = 500;
            
            if (Type == "S")
            {
                score = Convert.ToString(Lar[i]) + ".00";
            }
            else
            {
                score = Convert.ToString(Lar[i]);
            }
            if (score == "0.00" || score == "0") score = "";
            strCreate += "<div style='width: " + wdpx + ";margin-bottom: "+MarginBottom+";' class='chartLeft'>" + score + "</div>";
        }
        LG = strCreate;
        strCreate = "";



        for (int i = 0; i < RGLength; i++)
        {
            if (Convert.ToInt16(Rar[i]) ==0)
            {
                width = 0;
                wdpx = "0 !important;padding:0 !important ";
                MarginBottom = "35px";
            }
            else
            {
                width = Convert.ToInt16(Rar[i]) * 5 - 5;
                wdpx = width.ToString() + "px";
                MarginBottom = "28px";
            }

            if (width > 500) width = 500;
            

            if (Type == "S")
            {
                score = Convert.ToString(Rar[i]) + ".00";
            }
            else
            {
                score = Convert.ToString(Rar[i]);
            }
            if (score == "0.00") score = "";
            strCreate += "<div style='width: " + wdpx + ";margin-bottom: "+MarginBottom+";' class='chartRight'>" + score + "</div>";
        }

        RG = strCreate;

        strCreate = "";
        for (int i = 0; i < NMLength; i++)
        {
            strCreate += "<div class='names'>" + arNames[i].ToString() + "</div>";
        }

        Names = strCreate;
    }
}
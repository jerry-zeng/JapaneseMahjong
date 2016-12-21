using UnityEngine;
using System.Collections;


public class AgariInfo 
{
    public int han;
    public int fu;
    public string[] yakuNames;
    public YakuHandler[] hanteiYakus;
    public ScoreInfo scoreInfo;

    public int agariScore;

    public override string ToString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append( scoreInfo.ToString() );
        sb.Append( "\n" );

        sb.Append( "Yaku Names: \n" );

        for(int i = 0; i < yakuNames.Length; i++)
            sb.Append(  yakuNames[i] + "\n" );

        sb.Append( "Han: " + han.ToString() );
        sb.Append( "\n" );
        sb.Append( "Fu: " + fu.ToString() );
        sb.Append( "\n" );

        sb.Append( "AgariScore: " + agariScore.ToString() );
        sb.Append( "\n" );

        return sb.ToString();
    }
}

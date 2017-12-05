using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticsLab
{
    class PairWiseAlign
    {
        int MaxCharactersToAlign;
        private const int Bandwidth = 8; // the bandwidth value

        public PairWiseAlign()
        {
            // Default is to align only 5000 characters in each sequence.
            this.MaxCharactersToAlign = 5000;
        }

        public PairWiseAlign(int len)
        {
            // Alternatively, we can use an different length; typically used with the banded option checked.
            this.MaxCharactersToAlign = len;
        }

        /// <summary>
        /// this is the function you implement.
        /// </summary>
        /// <param name="sequenceA">the first sequence</param>
        /// <param name="sequenceB">the second sequence, may have length not equal to the length of the first seq.</param>
        /// <param name="banded">true if alignment should be band limited.</param>
        /// <returns>the alignment score and the alignment (in a Result object) for sequenceA and sequenceB.  The calling function places the result in the display appropriately.
        /// 
        public ResultTable.Result Align_And_Extract(GeneSequence sequenceA, GeneSequence sequenceB, bool banded)
        {
            ResultTable.Result result = new ResultTable.Result();
            int score;                                                       // place your computed alignment score here
            string[] alignment = new string[2];                              // place your two computed alignments here
            int sub = MaxCharactersToAlign;
            if (sequenceA.Sequence.Length < sub)
            {
                sub = sequenceA.Sequence.Length;
            }
            int sub2 = MaxCharactersToAlign;
            if (sequenceB.Sequence.Length < sub2)
            {
                sub2 = sequenceB.Sequence.Length;
            }

            // ********* these are placeholder assignments that you'll replace with your code  *******
            score = int.MaxValue;
            alignment[0] = "No Alignment Possible";
            alignment[1] = "No Alignment Possible";

            EditDistance editor;
            if (banded)
            {
                if (Math.Abs(sub2 - sub) > Bandwidth)
                {
                    result.Update(score, alignment[0], alignment[1]);                  // bundling your results into the right object type 
                    return (result);
                }
                editor = new EditDistance(sequenceA.Sequence.Substring(0,sub), sequenceB.Sequence.Substring(0, sub2));
                editor.setupBanded();
                //Console.WriteLine(editor.toString());
                alignment = editor.bandedResults();
                //Console.WriteLine(editor.toString());
                score = editor.value();
            } else
            {
                editor = new EditDistance(sequenceA.Sequence.Substring(0, sub), sequenceB.Sequence.Substring(0, sub2));
                editor.setupUnbanded();
                alignment = editor.results();
                score = editor.value();
            }
            
            // ***************************************************************************************
            

            result.Update(score,alignment[0],alignment[1]);                  // bundling your results into the right object type 
            return(result);
        }
    }
}

using System;

namespace BowlingScorer.Domain
{
    public class BowlingGame
    {
        private int[] rolls = new int[21];
        private int currentRoll = 0;
        private int completedFrames = 0;
        private int rollIndexInFrame = 0;
        private int remainingRolls = 20;
        private bool valid = true;

        public void Roll(int pins)
        {
            if (pins < 0 || pins > 10 || remainingRolls < 0)
            {
                valid = false;
            }
            else
            {
                if (completedFrames < 9)
                {
                    if (pins == 10)
                        remainingRolls--;

                    if (pins == 10 || rollIndexInFrame == 1)
                    {
                        completedFrames++;
                        rollIndexInFrame = 0;
                    }
                    else
                    {
                        rollIndexInFrame++;
                    }
                }
                else
                {
                    if (isStrike(rollIndexInFrame, pins) || isSpare(rollIndexInFrame, pins))
                        remainingRolls++;

                    rollIndexInFrame++;
                }
            }

            rolls[currentRoll++] = pins;
            remainingRolls--;
        }

        public Nullable<int> Score()
        {
            if (remainingRolls != 0 || !valid) return null;

            int score = 0;
            int rollIndex = 0;
            int currentFrame = 0;
            currentRoll = 0;
            int bonus = 0;

            for (currentFrame = 0; currentFrame < 10; currentFrame++)
            {
                if (isStrike(rollIndex))
                {
                    bonus = getStrikeBonus(rollIndex);

                    if (currentFrame == 9 && bonus > 10 && bonus != 20)
                        valid = false;

                    score += 10 + bonus;
                    rollIndex += 1;
                }
                else if (isSpare(rollIndex))
                {
                    score += 10 + getSpareBonus(rollIndex);
                    rollIndex += 2;
                }
                else
                {
                    int frameScore = getFrameScore(rollIndex);

                    if (frameScore > 10)
                        valid = false;

                    score += frameScore;
                    rollIndex += 2;
                }
            }

            if (!valid) return null;

            return score;
        }

        private bool isStrike(int rollIndex) => rolls[rollIndex] == 10;

        private bool isStrike(int rollIndex, int pins) => rollIndexInFrame == 0 && pins == 10;

        private int getStrikeBonus(int rollIndex) => rolls[rollIndex + 1] + rolls[rollIndex + 2];

        private bool isSpare(int rollIndex) => rolls[rollIndex] + rolls[rollIndex + 1] == 10;

        private bool isSpare(int rollIndexInFrame, int pins) => rollIndexInFrame == 1 && rolls[currentRoll - 1] + pins == 10;

        private int getSpareBonus(int rollIndex) => rolls[rollIndex + 2];

        private int getFrameScore(int rollIndex) => rolls[rollIndex] + rolls[rollIndex + 1];
    }
}

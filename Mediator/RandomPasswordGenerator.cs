using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mediator
{
    public static class RandomPasswordGenerator
    {
        private static char[] excludedCharacters = { '\\' };

        public static string Generate(int length, char minAllowedSymbol = 'A', char maxAllowedSymbol = 'z')
        {
            string generatedPassword = "";
            char previousCharacter = ' ';

            for(int i = 0; i < length; i++)
            {
                Random random = new Random();
                char randChar = (char)random.Next(minAllowedSymbol, maxAllowedSymbol);

                if(excludedCharacters.Contains(randChar))
                {
                    i--;
                    continue;
                }    

                if(randChar == previousCharacter)
                {
                    i--;
                    continue;
                }

                previousCharacter = randChar;
                generatedPassword += randChar;
            }

            return generatedPassword;
        }
    }
}

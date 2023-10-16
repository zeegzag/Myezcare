using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace HomeCareApi.Models.General
{
    public class OtpCodeGenerator
    {
        enum CharacterType
        {
            Uppercase,
            Lowercase,
            Special,
            Number
        }

        string _errDesc;
        private static readonly char[] Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly char[] Numbers = "1234567890".ToCharArray();
        private static readonly char[] Symbols = "@#$".ToCharArray();

        int _minimumLength, _maximumLength;
        bool _includeUpper, _includeLower, _includeNumber, _includeSpecial;
        private static readonly RNGCryptoServiceProvider Random = new RNGCryptoServiceProvider();
        private static readonly byte[] Bytes = new byte[4];
        string[] _characterTypes;

        #region Set Properties

        public bool IncludeUpper
        {
            get
            {
                return _includeUpper;
            }
            set
            {
                _includeUpper = value;
            }
        }

        public bool IncludeLower
        {
            get
            {
                return _includeLower;
            }
            set
            {
                _includeLower = value;
            }
        }

        public bool IncludeNumber
        {
            get
            {
                return _includeNumber;
            }
            set
            {
                _includeNumber = value;
            }
        }

        public bool IncludeSpecial
        {
            get
            {
                return _includeSpecial;
            }
            set
            {
                _includeSpecial = value;
            }
        }

        public int MinimumLength
        {
            get
            {
                return _minimumLength;
            }
            set
            {
                if (value > _maximumLength)
                {
                    throw new ArgumentOutOfRangeException("MinimumLength must be greater than MaximumLength");
                }
                _minimumLength = value;
            }
        }

        public int MaximumLength
        {
            get
            {
                return _maximumLength;
            }
            set
            {
                if (value < _minimumLength)
                {
                    throw new ArgumentOutOfRangeException("MaximumLength must be greater than MinimumLength");
                }
                _maximumLength = value;
            }
        }

        public string err
        { get { return this._errDesc; } }

        #endregion

        #region Generate Random Code


        public string GenerateRandomCode(int minimumLength, int maximumLength, bool includeSpecial, bool includeNumber, bool includeUpper, bool includeLower)
        {
            return GenerateRandomCode("", minimumLength, maximumLength, includeSpecial, includeNumber, includeUpper, includeLower);
        }

        /// <summary>
        /// Randomly creates a password.
        /// </summary>
        /// <returns>A random string of characters.</returns>
        public string GenerateRandomCode(string prefix, int minimumLength, int maximumLength, bool includeSpecial, bool includeNumber, bool includeUpper, bool includeLower)
        {

            _minimumLength = minimumLength;
            _maximumLength = maximumLength;
            _includeNumber = includeNumber;
            _includeSpecial = includeSpecial;
            _includeUpper = includeUpper;
            _includeLower = includeLower;
            _characterTypes = GetCharacterTypes();

            StringBuilder password = new StringBuilder(_maximumLength);

            //Get a random length for the password.
            int currentPasswordLength = GetNextNumber(_maximumLength);

            //Only allow for passwords greater than or equal to the minimum length.
            if (currentPasswordLength < _minimumLength)
            {
                currentPasswordLength = _minimumLength;
            }

            //Generate the password
            for (int i = 0; i < currentPasswordLength; i++)
            {
                password.Append(GetCharacter());
            }


            return prefix + password;
        }

        /// <summary>
        /// Determines which character types should be used to generate
        /// the current password.
        /// </summary>
        /// <returns>A string[] of character that should be used to generate the current password.</returns>
        private string[] GetCharacterTypes()
        {
            ArrayList characterTypes = new ArrayList();
            foreach (string characterType in Enum.GetNames(typeof(CharacterType)))
            {
                CharacterType currentType = (CharacterType)Enum.Parse(typeof(CharacterType), characterType, false);
                bool addType = false;
                switch (currentType)
                {
                    case CharacterType.Lowercase:
                        addType = IncludeLower;
                        break;
                    case CharacterType.Number:
                        addType = IncludeNumber;
                        break;
                    case CharacterType.Special:
                        addType = IncludeSpecial;
                        break;
                    case CharacterType.Uppercase:
                        addType = IncludeUpper;
                        break;
                }
                if (addType)
                {
                    characterTypes.Add(characterType);
                }
            }
            return (string[])characterTypes.ToArray(typeof(string));
        }

        /// <summary>
        /// Randomly determines a character type to return from the 
        /// available CharacterType enum.
        /// </summary>
        /// <returns>The string character to append to the password.</returns>
        private string GetCharacter()
        {
            string characterType = _characterTypes[GetNextNumber(_characterTypes.Length)];
            CharacterType typeToGet = (CharacterType)Enum.Parse(typeof(CharacterType), characterType, false);
            switch (typeToGet)
            {
                case CharacterType.Lowercase:
                    return Letters[GetNextNumber(Letters.Length)].ToString().ToLower();
                case CharacterType.Uppercase:
                    return Letters[GetNextNumber(Letters.Length)].ToString().ToUpper();
                case CharacterType.Number:
                    return Numbers[GetNextNumber(Numbers.Length)].ToString();
                case CharacterType.Special:
                    return Symbols[GetNextNumber(Symbols.Length)].ToString();
            }
            return null;
        }

        public static int GetNextNumber(int max)
        {
            if (max <= 0)
            {
                throw new ArgumentOutOfRangeException("max");
            }
            Random.GetBytes(Bytes);
            int value = BitConverter.ToInt32(Bytes, 0) % max;
            if (value < 0)
            {
                value = -value;
            }
            return value;
        }

        #endregion
    }
}
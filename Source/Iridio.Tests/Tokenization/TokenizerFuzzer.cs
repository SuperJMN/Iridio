using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bogus;
using Iridio.Tokenization;
using Optional.Collections;

namespace Iridio.Tests.Tokenization
{
    class TokenizerFuzzer
    {
        public TokenizerFuzzer()
        {
            var random = new Random();
            var faker = new Faker();
            dictionary = new Dictionary<SimpleToken, Func<string>>()
            {
                {SimpleToken.CloseBrace, () => "}"},
                {SimpleToken.OpenBrace, () => "{"},
                {SimpleToken.Integer, () => faker.Random.Number(1000).ToString()},
                {SimpleToken.Semicolon, () => ";"},
                {SimpleToken.Text, () => "\"" + faker.Random.AlphaNumeric(10) + "\"" },
                {SimpleToken.Double, () => random.NextDouble().ToString(CultureInfo.InvariantCulture) + "d"},
            };
        }

        private readonly IDictionary<SimpleToken, Func<string>> dictionary;


        public (string, IEnumerable<SimpleToken>) Generate(int numberOfTokens)
        {
            var tokens = TokenStream
                .Create(dictionary.Keys.ToList())
                .Take(numberOfTokens)
                .ToList();
            
            var str = string.Concat(tokens.Select(GetString));
            return (str, tokens);
        }

        private string GetString(SimpleToken simpleToken)
        {
            var value = DictionaryExtensions.GetValueOrNone(dictionary, simpleToken)
                .Map(toString => toString())
                .ValueOr("");
            return value;
        }
    }
}
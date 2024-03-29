﻿using System.Linq;
using CSharpFunctionalExtensions;
using Iridio.Core;
using Iridio.Parsing.Model;
using Iridio.Tokenization;
using MoreLinq.Extensions;
using Superpower;
using Zafiro.Core.Mixins;

namespace Iridio.Parsing
{
    public class Parser : IParser
    {
        public Result<IridioSyntax, ParsingError> Parse(string source)
        {
            try
            {
                var tokenization = Tokenizer.Create().Tokenize(source);
                var parsed = ParserDefinitions.Parser.Parse(tokenization);
                return Result.Success<IridioSyntax, ParsingError>(parsed);
            }
            catch (ParseException e)
            {
                var message = e.Message
                    .SkipUntil(c => c == ')')
                    .Skip(1).AsString();

                var position = new Position(e.ErrorPosition.Line, e.ErrorPosition.Column);
                return Result.Failure<IridioSyntax, ParsingError>(new ParsingError(position, message));
            }
        }
    }
}
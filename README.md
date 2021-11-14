# Iridio

## What's Iridio?

Iridio is a scripting language, nothing more, nothing less :)

I invented it because I needed more flexibility for my [Deployer](https://www.github.com/SuperJMN/Deployer). I needed some scripting language easy enough and flexible enough.

## How is this useful for me?

If you need to execute your own scripts with your own built-in functions, Iridio might be a good choice.

## How do I use it?

1. Add a reference in you project to the Nuget [Iridio.Runtime](https://www.nuget.org/packages/Iridio.Runtime/)
2. Create an instance of `IridioShell` and execute some script with it

   Take this example:

   ```csharp
	var script = @"Main { a = 3 * 5; }";
	var shell = new IridioShell();
	ExecutionSummary execution = await shell.Run(SourceCode.FromString(script));
	Console.WriteLine(result.Value.Variables["a"]);
   ```
   
   It will execute the script an returns a list of the variables in the script with their last value. Running this example will return the value **15** into **a**

As you see, it's extremely easy to use.

## Getting started

If you want to get started, please, check the [Wiki](https://github.com/SuperJMN/Iridio/wiki) :)

## Acknowledgements

In some way or another, you made this possible. Thank you all!

- [Robert C. Martin (Uncle Bob)](https://twitter.com/unclebobmartin)
- [Nicholas Blumhardt](https://twitter.com/nblumhardt)
- [Zoran Horvat](https://twitter.com/zoranh75). 
- [Immo Landwerth](https://twitter.com/terrajobst)
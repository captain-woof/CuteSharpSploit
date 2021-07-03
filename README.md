# CuteSharpSploit

<p align="center">
  <img width="450" height="300" src="https://drive.google.com/uc?export=download&id=1W25A9KB8INfVq1TtteixUAhx2ho1rkXr">
</p>

### Introduction 🔪

**CuteSharpSploit is an executable wrapper around [SharpSploit](https://github.com/cobbr/SharpSploit)**, a .NET post-exploitation library written in C# by Ryan Cobb. Most of the features exported by the SharpSploit library have been implemented.

### Usage 🔪

**The application provides a CLI, and performs actions thorugh loadable modules**. Type `help` to get a list of available commands.
```
1. load | use <module name>
2. set <option name> <option value>
3. info | options
4. run
5. search <module name (substring)>
6. show all
7. powershell
8. help
9. exit
```

### Building 🔪

Since the SharpSploit project has been implemented in .NET Framework 3.5 and 4.0 only, you need to use the proper references.

**For targeting .NET Framework 3.5 platform, add references to all the DLLs within `sharpsploit-dlls\net35`. Similarly, for targeting .NET Framework 4.0 platform, add references to only all the DLLs within `sharpsploit-dlls\net40`.** *(Default is v3.5)*

Alternatively, you can build your own set of SharpSploit libraries from [their repo](https://github.com/cobbr/SharpSploit), then add them as references as described above.

### Releases 🔪

I have decided to not release any binaries, because...well, you know ;)

### Credits 🔪

Since this executable wrapper heavily uses the SharpSploit library, therefore, all the credit goes to the author and all the contributors of SharpSploit.

### Author 🏃

##### CaptainWoof

![Twitter Follow](https://img.shields.io/twitter/follow/realCaptainWoof)

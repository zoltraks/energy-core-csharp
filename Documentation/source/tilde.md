Tilde Coloring Engine
=====================

Introduction
------------

Simple elegant text coloring engine for console programs.
Based on **Empty Page #0** color engine.

Color may be specified by its number or name surrounded by tilde (~) character.
One tilde followed by a character other than a letter or number or a fragment consisting of two or more tilde characters will not be formatted.

Colors that can be used are the same as on a standard command console. There are 15 different colours plus black.

Dark colour identifiers are preceded by the letter 'd' (dark).
In a similar way, bright-colour identifiers can be preceded by the letter 'l' (light).

Color changes one by one are ignored. Only the last defined color will be used.

Current color is remembered before writing color text and restored after writing. Special *color* ~0~ may be used to force setting back original color inside text.

Example
-------

```csharp
Energy.Core.Tilde.WriteLine("~yellow~Hello, ~cyan~world~white~!");
```

![](../media/tilde01.png)

```csharp
Energy.Core.Tilde.Write(" ~1~{1}~2~{2}~3~{3}~4~{4}~5~{5}~6~{6} ", null
    , 1, 2, 3, 4, 5, 6);
```

![](../media/tilde02.png)

```csharp
Energy.Core.Tilde.WriteLine("You can use ~`~yellow~`~ to mark text ~yellow~yellow~0~.");
```

![](../media/tilde03.png)

```csharp
Energy.Core.Tilde.WriteLine("~yellow~Welcome to ~blue~ReadLine ~yellow~example~white~!");
Console.ForegroundColor = ConsoleColor.Magenta;
Energy.Core.Tilde.Write("Write ~c~something~0~: ~magenta~");
while (true)
{
    string input = Energy.Core.Tilde.ReadLine();
    if (input == null)
    {
        Thread.Sleep(500);
        continue;
    }
    else
    {
        Energy.Core.Tilde.WriteLine("You have written ~green~{0}~0~...", input);
        break;
    }
}
Energy.Core.Tilde.Pause();
```

![](../media/tilde04.png)

Color table
-----------

List of available colors:

<table>
<thead>
    <th>Numeric</th><th>Long</th>           <th>Short</th>   <th></th>
</thead>
<tbody>
<tr><td>~0~</td>    <td>~default~</td>      <td></td>        <td>Default color</td></tr>
<tr><td>~1~</td>    <td>~darkblue~</td>     <td>~db~</td>    <td>Dark blue</td></tr>
<tr><td>~2~</td>    <td>~darkgreen~</td>    <td>~dg~</td>    <td>Dark green</td></tr>
<tr><td>~3~</td>    <td>~darkcyan~</td>     <td>~dc~</td>    <td>Dark cyan</td></tr>
<tr><td>~4~</td>    <td>~darkred~</td>      <td>~dr~</td>    <td>Dark red</td></tr>
<tr><td>~5~</td>    <td>~darkmagenta~</td>  <td>~dm~</td>    <td>Dark magenta</td></tr>
<tr><td>~6~</td>    <td>~darkyellow~</td>   <td>~dy~</td>    <td>Dark yellow</td></tr>
<tr><td>~7~</td>    <td>~gray~</td>         <td>~s~</td>     <td>Gray / Silver</td></tr>
<tr><td>~8~</td>    <td>~darkgray~</td>     <td>~ds~</td>    <td>Dark gray / Dark silver</td></tr>
<tr><td>~9~</td>    <td>~blue~</td>         <td>~b~</td>     <td>Blue</td></tr>
<tr><td>~10~</td>   <td>~green~</td>        <td>~g~</td>     <td>Black</td></tr>
<tr><td>~11~</td>   <td>~cyan~</td>         <td>~c~</td>     <td>Cyan</td></tr>
<tr><td>~12~</td>   <td>~red~</td>          <td>~r~</td>     <td>Red</td></tr>
<tr><td>~13~</td>   <td>~magenta~</td>      <td>~m~</td>     <td>Magenta</td></tr>
<tr><td>~14~</td>   <td>~yellow~</td>       <td>~y~</td>     <td>Yellow</td></tr>
<tr><td>~15~</td>   <td>~white~</td>        <td>~w~</td>     <td>White</td></tr>
<tr><td>~16~</td>   <td>~black~</td>        <td>~k~</td>     <td>Black / Carbon</td></tr>
</tbody>
</table>

Hints
-----

For escaping tilde character in text use grave accent character ~\` ... \`~.

For example ~\`~color~\`~ will be printed as ~color~.
You may optionally use double grave accent character \`\` to include hash sign inside brackets ~\` \`\` \`~.


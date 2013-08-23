using System.Collections.Generic;
using System.Linq;

public static class StringExtensions
{
    public static bool AddIfDoesntExists<A, B>(this IDictionary<A, B> dic, A key, B value)
    {
        var exists = dic.ContainsKey(key);
        if (!exists)
            dic.Add(key, value);

        return exists;
    }

    public static bool OptionPresent(this IList<string> args, string option, ref string setting)
    {
        var arg = args.FirstOrDefault(a => a.StartsWith("-"+option));
        var optionPresent = arg != null;        

        if (optionPresent)
            setting = arg.Replace("-"+option+"=", "");            
        
        return optionPresent;
    }

    public static bool OptionPresent(this IList<string> args, string option, ref int setting)
    {
        var arg = args.FirstOrDefault(a => a.StartsWith("-" + option));
        var optionPresent = arg != null;

        if (optionPresent)
        {
            int posbbleSetting;

            if (int.TryParse(arg.Replace("-" + option + "=", ""), out posbbleSetting))
                setting = posbbleSetting;
        }
            

        return optionPresent;
    }
}
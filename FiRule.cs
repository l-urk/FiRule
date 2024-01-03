using System;
using NetFwTypeLib;

class Program
{
    static void Main(string[] args)
    {
        string ruleName = ParseCommandLine(args);

        if (string.IsNullOrEmpty(ruleName))
        {
            Console.WriteLine("Please provide the firewall rule name using the -r flag.");
            return;
        }

        try
        {
            DeleteFirewallRuleByName(ruleName);
            Console.WriteLine($"Firewall rule '{ruleName}' deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting firewall rule: {ex.Message}");
        }
    }

    static string ParseCommandLine(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals("-r", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                return args[i + 1];
            }
        }

        return null;
    }

    static void DeleteFirewallRuleByName(string ruleName)
    {
        Type type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
        dynamic firewallPolicy = Activator.CreateInstance(type);

        var rules = firewallPolicy.Rules as INetFwRules;

        foreach (INetFwRule rule in rules)
        {
            if (rule.Name == ruleName)
            {
                rules.Remove(rule.Name);
                return;
            }
        }

        throw new ArgumentException($"Firewall rule '{ruleName}' not found.");
    }
}

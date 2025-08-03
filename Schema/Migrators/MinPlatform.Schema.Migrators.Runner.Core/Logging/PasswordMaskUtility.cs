namespace MinPlatform.Schema.Migrators.Runner.Logging
{
    using System.Text.RegularExpressions;

    public class PasswordMaskUtility : IPasswordMaskUtility
    {
        private static readonly Regex matchPwd = new Regex("(?<Prefix>.*)(?<Key>PWD\\s*=\\s*|PASSWORD\\s*=\\s*)(?<OptionalValue>((?<None>(;|;$|$))|(?<Value>(([^;]+$|[^;]+)))(?<ValueTerminator>$|;)))(?<Postfix>.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public string ApplyMask(string connectionString)
        {
            return matchPwd.Replace(connectionString, "${Prefix}${Key}********${None}${ValueTerminator}${Postfix}");
        }
    }
}

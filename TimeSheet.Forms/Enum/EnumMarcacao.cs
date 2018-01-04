using System.ComponentModel;

namespace TimeSheet.Forms.Enum
{
    public enum EnumMarcacao
    {
        [Description("Entrada")]
        Entrada = 1,
        [Description("SaidaAlmoco")]
        SaidaAlmoco = 2,
        [Description("RetornoAlmoco")]
        RetornoAlmoco = 3,
        [Description("Saida")]
        Saida = 4
    }
}

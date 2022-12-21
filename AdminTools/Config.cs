using System.ComponentModel;

namespace AdminTools
{
    public class Config
    {
        [Description("Should the tutorial class be in God Mode? Default: true")]
        public bool GodTuts { get; set; } = true;
    }
}

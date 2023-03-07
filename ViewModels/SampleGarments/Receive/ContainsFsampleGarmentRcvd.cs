namespace DenimERP.ViewModels.SampleGarments.Receive
{
    public class ContainsFsampleGarmentRcvd
    {
        public double? Remainings { get; }
        public string FABCODE { get; }

        public ContainsFsampleGarmentRcvd(double? remainings, string fabcode)
        {
            Remainings = remainings;
            FABCODE = fabcode;
        }
    }
}

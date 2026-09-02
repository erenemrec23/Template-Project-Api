namespace QrAssignment.Domain.Entity.System
{
    public enum SystemRegionLevel
    {
        Country = 1,       // Türkiye (ParentId = null)
        City = 2,          // İstanbul (ParentId = Türkiye'nin Id'si)
        District = 3,      // Kadıköy (ParentId = İstanbul'un Id'si)
        //Neighborhood = 4   // Moda (ParentId = Kadıköy'ün Id'si)
    }
}

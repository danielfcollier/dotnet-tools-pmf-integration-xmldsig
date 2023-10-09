using Handlers;

namespace Models;

public static class DBHandler
{
    public static async Task<Partner> GetPartnerData(string filepath, string partnerId)
    {
        List<Partner>? partners = await JsonHandler.ReadAll<List<Partner>>(filepath);
        if (partners is null) {
            throw new Exception("Partners data can not be null!");
        }

        var partner = partners.Where(partner => partner.Id == partnerId).First();

        return partner;
    }
}
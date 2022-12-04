namespace Services;

public static class CityCodesService
{
    private static readonly int BASE_CITY_CODE = 4205407; // FLORIANÓPOLIS

    public static int GetCfps(int cityCode)
    {
        Dictionary<string, int> CfpsTable = new()
            {
                { "SameCity", 9201 },
                { "SameState", 9202 },
                { "OtherState", 9203 },
                { "OtherCountry", 9204 },
            };

        if (cityCode == BASE_CITY_CODE)
        {
            return CfpsTable["SameCity"];
        }

        string baseStateCode = $"{BASE_CITY_CODE}";
        string cityStateCode = $"{cityCode}";

        bool hasSameStateBase = baseStateCode.Substring(0, 2) == cityStateCode.Substring(0, 2);

        if (hasSameStateBase)
        {
            return CfpsTable["SameState"];
        }

        return CfpsTable["OtherState"];
    }
}
using System.Text.Json;
using DPBack.Application.Contracts;
using DPBack.Application.Interfaces;
using DPBack.Application.PricingConfigurations;
using DPBack.Domain.Enums;


namespace DPBack.Application.Services;

public class PriceCalcService : IPriceCalcService
{
    public async Task<PriceResultDto> CalculatePrice(GetPriceDto request)
    {
        switch (request.Type)
        {
            case OrderItemType.Businesscard:
            {
                var config = JsonSerializer.Deserialize<BusinesscardConfig>(request.Configuration.GetRawText());
                float price = 0.4f;
                switch (config.Thickness)
                {
                    case "250":
                    {
                        break;
                    }
                    case "300":
                    {
                        price += 0.1f;
                        break;
                    }
                    case "350":
                    {
                        price += 0.2f;
                        break;
                    }
                }

                switch (config.Coating)
                {
                    case "glossy":
                    {
                        break;
                    }
                    case "matte":
                    {
                        break;
                    }
                    case "softtouch":
                    {
                        price += 0.1f;
                        break;
                    }
                }
                switch (config.AversPrint)
                {
                    case 0: // BW
                    {
                        price += 0.05f;
                        break;
                    }
                    case 1: //Color
                    {
                        price += 0.07f;
                        break;
                    }
                }
                switch (config.ReversPrint)
                {
                    case 0: // BW
                    {
                        price += 0.05f;
                        break;
                    }
                    case 1: //Color
                    {
                        price += 0.07f;
                        break;
                    }
                    case 2: //no print
                    {
                        break;
                    }
                }
                return new PriceResultDto {PricePerUnity = price};
            }
        }

        throw new Exception("wrong config");
    }
}
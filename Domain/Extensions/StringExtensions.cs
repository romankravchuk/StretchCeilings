﻿using System.Collections.Generic;
using System.Linq;
using StretchCeilings.Domain.Models.Enums;

namespace StretchCeilings.Domain.Extensions
{
    public static class StringExtensions
    {
        private static readonly Dictionary<string, OrderStatus> OrderStatuses;
        private static readonly Dictionary<string, TextureType> TextureTypes;
        private static readonly Dictionary<string, ColorType> ColorTypes;
        private static readonly Dictionary<string, Country> Countries;

        static StringExtensions()
        {
            OrderStatuses = new Dictionary<string, OrderStatus>()
            {
                { "Отменен", OrderStatus.Canceled },
                { "Выполнен", OrderStatus.Finished },
                { "Ожидает ответа заказчика", OrderStatus.WaitingForCustomerAnswer },
                { "Ожидает выполения", OrderStatus.WaitingForExecution },
                { "Ожидает результатов замеров", OrderStatus.WaitingForMeasurements },
                { "Ожидает оплаты", OrderStatus.WaitingForPaid },
                { "Ожидает пирбытия потолков", OrderStatus.WaitingForCeilings },
            };

            TextureTypes = new Dictionary<string, TextureType>
            {
                { "Тканевый", TextureType.Fabric },
                { "Матовый", TextureType.Matte },
                { "Глянцевый", TextureType.Glossy },
                { "Сатиновый", TextureType.Satin }
            };

            ColorTypes = new Dictionary<string, ColorType>
            {
                { "Белый", ColorType.White},
                { "Цветной", ColorType.Colored },
            };
            Countries = new Dictionary<string, Country>()
            {
                { "Россия", Country.Russia },
                { "США", Country.USA },
                { "Англия", Country.UK },
                { "Германия", Country.Germany },
                { "Китай", Country.China },
            };
        }

        public static Country ToCountry(this string value)
        {
            return Countries.FirstOrDefault(k => k.Key == value).Value;
        }

        public static OrderStatus ToOrderStatus(this string value)
        {
            return OrderStatuses.FirstOrDefault(k => k.Key == value).Value;
        }

        public static TextureType ToTextureType(this string value)
        {
            return TextureTypes.FirstOrDefault(k => k.Key == value).Value;
        }

        public static ColorType ToColorType(this string value)
        {
            return ColorTypes.FirstOrDefault(k => k.Key == value).Value;
        }
    }
}

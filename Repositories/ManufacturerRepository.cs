﻿using System.Collections.Generic;
using System.Linq;
using StretchCeilings.DataAccess;
using StretchCeilings.Models;
using StretchCeilings.Models.Enums;

namespace StretchCeilings.Repositories
{
    public class ManufacturerRepository
    {
        public static List<Manufacturer> GetAll(out int rows)
        {
            using (var db = new StretchCeilingsContext())
            {
                var queryable = db.Manufacturers.Where(x => x.DeletedDate == null);
                rows = 0;

                if (queryable.Any()) 
                    rows = queryable.Count();

                return queryable.ToList();
            }
        }

        public static List<Manufacturer> GetAll(Manufacturer firstFilter, int count, int page, out int rows)
        {
            using (var db = new StretchCeilingsContext())
            {
                var queryable = db.Manufacturers.Where(x => x.DeletedDate == null);
                rows = 0;

                if (firstFilter.Id != 0)
                    queryable = queryable.Where(x => x.Id == firstFilter.Id);
                
                if (firstFilter.Address != null)
                    queryable =  queryable.Where(x => x.Address == firstFilter.Address);

                if (firstFilter.Country != Country.Unknown)
                    queryable = queryable.Where(x => x.Country == firstFilter.Country);

                if (firstFilter.Name != null)
                    queryable = queryable.Where(x => x.Name == firstFilter.Name);
                
                if (queryable.Any() == false) 
                    return queryable.ToList();

                rows = queryable.Count();
                return queryable.ToList().Skip((page - 1) * count).Take(count).ToList();
            }
        }

        public static Manufacturer GetById(int id)
        {
            using (var db = new StretchCeilingsContext())
            {
                return db.Manufacturers.First(o => o.Id == id);
            }
        }
    }
}

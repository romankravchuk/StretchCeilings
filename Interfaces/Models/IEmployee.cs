﻿using System.Collections.Generic;
using stretch_ceilings_app.Data.Models;

namespace stretch_ceilings_app.Interfaces.Models
{
    public interface IEmployee
    {
        void Add();
        void Update();
        void Delete();
        List<TimeTable> GetSchedule();
        List<Order> GetOrders(int count, int pages);
        List<Service> GetServices(int count, int pages);
    }
}

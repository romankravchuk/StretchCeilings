﻿namespace stretch_ceilings_app.Interfaces
{
    public interface IRoom
    {
        void Add();
        void Update();
        void Delete();
        string GetPlane();
        string AddPlane(string path);
        string ChangePlane(string path);
    }
}

namespace Staff_Project.Models.Menu
{
    using System;

    public interface IMenu
    {
        public Tuple<string, int> PrintMenu();
    }
}

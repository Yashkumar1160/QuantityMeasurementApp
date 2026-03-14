using QuantityMeasurementApp.Controllers;
using QuantityMeasurementApp.Menu;
using QuantityMeasurementAppRepositories;
using QuantityMeasurementApp.Services;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppRepositories.Repositories;
using QuantityMeasurementAppServices.Interfaces;
using QuantityMeasurementApp.Interfaces;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementApp
    {
        private static QuantityMeasurementApp instance;
        private static readonly object lockObject = new object();

        private QuantityMeasurementController controller;
        private IMenu menu;


        private QuantityMeasurementApp()
        {
            //Singleton pattern 
            IQuantityMeasurementRepository repository =QuantityMeasurementCacheRepository.GetInstance();

            //Factory pattern
            IQuantityMeasurementService service =new QuantityMeasurementServiceImpl(repository);

            //Factory pattern
            controller = new QuantityMeasurementController(service);

            menu=new QuantityMenu(controller);
        }

        //Method to get the instance of QuantityMeasurementApp class
        public static QuantityMeasurementApp GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new QuantityMeasurementApp();
                    }
                }
            }
            return instance;
        }

        //Method to Start the menu 
        public void Start()
        {
            menu.Run();
        }
    }
}
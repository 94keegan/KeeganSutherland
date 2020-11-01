using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Core.Pages
{
    public class WorkoutModel : PageModel
    {
        private readonly ILogger<WorkoutModel> _logger;

        public WorkoutModel(ILogger<WorkoutModel> logger)
        {
            _logger = logger;
        }

        public List<string> SetList = new List<string>();
        public Workout Workout = new Workout();

        public void OnGet([FromQuery] Workout workout)
        {
            Workout = workout;
            GenerateWorkout(workout);
        }

        private void GenerateWorkout(Workout workout)
        {
            List<string> coreCenter = GetSets("CoreCenter");
            List<string> coreSide = GetSets("CoreSide");
            Random rnd = new Random();

            while(SetList.Count < workout.NumberOfSets)
            {
                if(SetList.Count + 1 != workout.NumberOfSets) // Prevents extra set when selecting coreSide
                {
                    Workout.SetType setType = (Workout.SetType)rnd.Next(1, 3);
                    generateRandomSet(setType == Workout.SetType.CoreCenter ? coreCenter : coreSide, setType, rnd);
                }
                else
                {
                    generateRandomSet(coreCenter, Workout.SetType.CoreCenter, rnd);
                }
            }
        }

        private void generateRandomSet(List<string> coreList, Workout.SetType coreType, Random rnd)
        {
            int coreSideRandomIndex = rnd.Next(coreList.Count);
            switch (coreType)
            {
                case Workout.SetType.CoreCenter:
                    SetList.Add(coreList[coreSideRandomIndex]);
                    break;
                case Workout.SetType.CoreSide:
                    SetList.Add($"{coreList[coreSideRandomIndex]} Right");
                    SetList.Add($"{coreList[coreSideRandomIndex]} Left");
                    break;
            }
            coreList.RemoveAt(coreSideRandomIndex);
        }

        private List<string> GetSets(string fileName)
        {
            try
            {
                return System.IO.File.ReadAllText($"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}/wwwroot/text/{fileName}.txt", Encoding.UTF8).Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return new List<string>();
        }
    }
}

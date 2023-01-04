﻿using Core.Interfaces;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Reservation.Attributes;
using Reservation.Attributes.ActionFilters;
using Reservation.consts;
using Reservation.Models;
using System.Diagnostics;

namespace Reservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRestaurantService reservationService;
        public HomeController(ILogger<HomeController> logger, IRestaurantService r)
        {
            reservationService = r;
            _logger = logger;
            
        }

      
      
        public IActionResult Index()
        {
          
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
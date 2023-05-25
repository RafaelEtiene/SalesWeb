﻿using Microsoft.AspNetCore.Mvc;
using SalesWebApp.Models;
using SalesWebApp.Models.ViewModel;
using SalesWebApp.Services;
using SalesWebApp.Services.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;

namespace SalesWebApp.Controllers
{
    public class SellersController : Controller
    {

        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }
        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                return View(seller);
            }
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { Message = "Id not provided" });

            var seller = _sellerService.FindById(id.Value);

            if (seller == null)
                return RedirectToAction(nameof(Error), new { Message = "Id not found" });

            return View(seller)
;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { Message = "Id not provided" });

            var seller = _sellerService.FindById(id.Value);

            if (seller == null)
                return RedirectToAction(nameof(Error), new { Message = "Id not found" });

            return View(seller);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { Message = "Id not provided" });
            }
            var validSeller = _sellerService.FindById(id.Value);
            if (validSeller == null)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel{ Message = "Id not found" });
            }

            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = validSeller, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id mismatch" });
            }
            try
            {
                if (!ModelState.IsValid)
                {
                    var departments = _departmentService.FindAll();
                    var viewModel = new SellerFormViewModel { Departments = departments, Seller = seller };
                    return View(viewModel);
                }
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { Message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { Message = e.Message });
            }

        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel() { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(viewModel);
        }
    }
}

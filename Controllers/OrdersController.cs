﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPChushka.Data;
using ASPChushka.Models;

namespace ASPChushka.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ChushkaContext _context;

        public OrdersController(ChushkaContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var chushkaContext = _context.Orders
                .Include(o => o.Product)
                .Include(o => o.User);
            return View(await chushkaContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        //с иточване на свързаните данни от таблици Products ,  Users
        // GET: Orders/Create
        public IActionResult Create()
        {
            OrdersVM model = new OrdersVM();
            model.Products = _context.Products.Select(pr => new SelectListItem
            {
                Value = pr.Id.ToString(),
                Text = pr.Name,
                Selected = pr.Id == model.ProductId
            }).ToList();
            model.Users = _context.Users.Select(user => new SelectListItem
            {
                Value = user.Id.ToString(),
                Text = user.FullName,
                Selected = user.Id == model.UserId
            }).ToList();

            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View(model);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("ProductId,UserId,OrderedOn")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            OrdersVM model = new OrdersVM();
            model.Products = _context.Products.Select(pr => new SelectListItem
            {
                Value = pr.Id.ToString(),
                Text = pr.Name,
                Selected = pr.Id == model.ProductId
            }).ToList();
            model.Users = _context.Users.Select(user => new SelectListItem
            {
                Value = user.Id.ToString(),
                Text = user.FullName,
                Selected = user.Id == model.UserId
            }).ToList();

            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(model);   //return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //1. зареждам искания запис от БД
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            //2. Създавм модела, с който ще визуализирам за промяна на стойностите
            //3. Пълня от БД в полетата на екрана
            OrdersVM model = new OrdersVM();
            model.Id = order.Id;
            model.ProductId = order.ProductId;
            model.OrderedOn = order.OrderedOn;
            model.UserId = order.UserId;
            // >>> зареждаме падащ списък с всички продукти от БД
            model.Products = _context.Products.Select(pr => new SelectListItem
            {
                Value = pr.Id.ToString(),
                Text = pr.Name,
                Selected = pr.Id == model.ProductId
            }).ToList();
            // >>> зареждаме падащ списък с всички потребители от БД
            model.Users = _context.Users.Select(user => new SelectListItem
            {
                Value = user.Id.ToString(),
                Text = user.FullName,
                Selected = user.Id == model.UserId
            }).ToList();
            //4. стартирам изгледа с напълнения модел
            return View(model);


            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", order.ProductId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", order.UserId);
            //return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,UserId,OrderedOn")] OrdersVM order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            //1. Намирам записа в БД >>> с модела от Data
            Order modelToDB = await _context.Orders.FindAsync(id);
            if (modelToDB == null)
            {
                return NotFound();
            }
           
            if (!ModelState.IsValid) //ако моделът не е ОК
            {
                //презареждаме страницата
                return View(modelToDB);
            }
            //2. Прехвърлям всичко в модела за БД .... готвим се за запис в БД
            modelToDB.Id = order.Id;
            modelToDB.ProductId = order.ProductId;
            modelToDB.UserId = order.UserId;
            modelToDB.OrderedOn = order.OrderedOn;
            //modelToDB.Product =await _context.Products.FindAsync(order.ProductId);
            //modelToDB.User = await _context.Users.FindAsync(order.UserId);

            //3. ЗАПИС в БД          
            try
            {
                _context.Update(modelToDB);   //  a NE  (order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(modelToDB.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //4. Извикваме Details на актуализирания запис
            return RedirectToAction("Details", new { id = id });


            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", order.ProductId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", order.UserId);
            //return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}

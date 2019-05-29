﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ALBLOG.Models;
using ALBLOG.Domain.Service;
using System.Text;
using ALBLOG.Domain.Model;

namespace ALBLOG.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int index = 1)
        {
            int postNumOfOnePage = 10;
            PostService postService = new PostService();
            var allPosts = postService.GetAllPosts(i => i.IsDraft == false);
            var posts = allPosts.Skip((index - 1) * 10).Take(postNumOfOnePage).ToList();
            if (posts.Count == 0)
            {
                posts = allPosts.Take(10).ToList();
                index = 1;
            }
            ViewData.Add("haveNext", allPosts.Count() > index * postNumOfOnePage ? "true" : "false");
            ViewData.Add("haveLast", index > 1 ? "true" : "false");
            ViewData.Add("posts", posts);
            ViewData.Add("sum", allPosts.Count() % postNumOfOnePage != 0 ? allPosts.Count() / postNumOfOnePage + 1 : allPosts.Count() / postNumOfOnePage);
            ViewData.Add("page", index);
            return View();
        }

        public IActionResult CV()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Post(string title)
        {
            PostService postService = new PostService();
            var post = postService.GetPost(i => i.Title == title);
            if (post == null)
                return RedirectToAction("Index", "Home");
            string tags = "";
            post.Tags.ForEach(i => tags += i + " ");
            ViewData.Add("date", post.Date.AddHours(8).ToString("yyyy-MM-dd HH:mm"));
            ViewData.Add("name", post.UserName);
            ViewData.Add("tags", tags);
            ViewData.Add("title", post.Title);
            ViewData.Add("context", post.Context);
            return View();
        }

        [HttpGet]
        public IActionResult Tag(string name, int index = 1)
        {
            int postNumOfOnePage = 10;
            PostService postService = new PostService();
            var allPosts = postService.GetAllPosts(i => i.IsDraft == false && i.Tags.Contains(name));
            var posts = allPosts.Skip((index - 1) * 10).Take(postNumOfOnePage).ToList();
            if (posts.Count == 0)
            {
                posts = allPosts.Take(10).ToList();
                index = 1;
            }
            ViewData.Add("Title", name);
            ViewData.Add("haveNext", allPosts.Count() > index * postNumOfOnePage ? "true" : "false");
            ViewData.Add("haveLast", index > 1 ? "true" : "false");
            ViewData.Add("posts", posts);
            ViewData.Add("sum", allPosts.Count() % postNumOfOnePage != 0 ? allPosts.Count() / postNumOfOnePage + 1 : allPosts.Count() / postNumOfOnePage);
            ViewData.Add("page", index);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}

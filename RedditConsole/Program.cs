using RedditSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RedditConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var reddit = new Reddit(false);
            //var user = reddit.LogIn("TylerH", "Password12", false);
            //var subreddit = reddit.GetSubreddit("/r/sandbox/");
        }

        public void Redditlogin()
        {
            var reddit = new Reddit(false);
            var user = reddit.LogIn("TylerH", "Boomer12", false);
            var subreddit = reddit.GetSubreddit("/r/sandbox/");
        }

        public void PostDailySCRUM(RedditSharp.Things.Subreddit subreddit)
        {
            var newestPost = subreddit.Posts.OrderByDescending(x => x.Created).First();
            newestPost.Comment($"SCRUM Update for {DateTime.Now.DayOfWeek}, {DateTime.Now.ToString("D")}");
        }

        public void PostMonthlySCRUM(RedditSharp.Things.Subreddit subreddit)
        {
            subreddit.SubmitTextPost($"Daily SCRUM - {DateTime.Now.ToString("y")}", $"# SCRUM updates for {DateTime.Now.ToString("y")}");
        }

       

    }
}

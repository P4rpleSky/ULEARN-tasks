using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rocket_bot
{
    public partial class Bot
    {
        private Task<Tuple<Turn, double>> SearchBestMoveAsync(Rocket rocket)
        {
            var task = new Task<Tuple<Turn, double>>(
                () => SearchBestMove(rocket, new Random(random.Next()), iterationsCount / threadsCount));
            task.Start();
            return task;
        }

        public Rocket GetNextMove(Rocket rocket)
        {
            var func = new Func<Rocket, Random, int, Tuple<Turn, double>>(SearchBestMove);
            var tasks = new Task<Tuple<Turn, double>>[threadsCount];
            for (int i = 0; i < threadsCount; ++i)
                tasks[i] = SearchBestMoveAsync(rocket);
            Task.WaitAll(tasks);
            var bestMove = tasks.Select(x => x.Result)
                .OrderByDescending(x => x.Item2)
                .First();
            var newRocket = rocket.Move(bestMove.Item1, level);
            return newRocket;
        }
    }
}
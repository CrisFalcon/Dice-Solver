using System.Collections.Generic;
using System;
using System.Linq;

public class DiceSolver
{
    public IEnumerable<T> AStar<T>(T start, Func<T, bool> satisfies, Func<T, IEnumerable<T>> getNeighbors, Func<T, float> heuristic)
    {
        Dictionary<T, float> frontier = new Dictionary<T, float>();
        frontier.Add(start, 0);

        Dictionary<T, T> cameFrom = new Dictionary<T, T>();
        cameFrom.Add(start, default);

        Dictionary<T, int> costSoFar = new Dictionary<T, int>();
        costSoFar.Add(start, 0);

        T current = default;
        int watchdog = 0;

        while (frontier.Count != 0)
        {
            watchdog++;
            if (watchdog > 200) return Enumerable.Empty<T>();

            current = frontier.OrderBy(x => x.Value).Select(x => x.Key).First();
            frontier.Remove(current);

            if (satisfies(current))
            {
                var path = FList.Create(current);
                while (!current.Equals(start))
                {
                    current = cameFrom[current];
                    path += current;
                }
                return path.Where(x => !x.Equals(start)).Reverse();
            }

            var neighbors = getNeighbors(current);
            int newCost = costSoFar[current] + 1;

            foreach (var next in neighbors)
            {
                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Add(next, newCost + heuristic(next));
                    costSoFar.Add(next, newCost);
                    cameFrom.Add(next, current);
                }
                else if (newCost < costSoFar[current])
                {
                    if (frontier.ContainsKey(next)) frontier[next] = newCost + heuristic(next);
                    else frontier.Add(next, newCost + heuristic(next));

                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                }
            }
        }

        return Enumerable.Empty<T>();
    }
}

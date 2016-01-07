import java.util.*;
import java.util.regex.Pattern;

public class Tennis {

    private static final Pattern pattern = Pattern.compile(" - ");

    static int lca(int[] match, int[] base, int[] p, int a, int b) {
        boolean[] used = new boolean[match.length];
        while (true) {
            a = base[a];
            used[a] = true;
            if (match[a] == -1) break;
            a = p[match[a]];
        }
        while (true) {
            b = base[b];
            if (used[b]) return b;
            b = p[match[b]];
        }
    }

    static void markPath(int[] match, int[] base, boolean[] blossom, int[] p, int v, int b, int children) {
        for (; base[v] != b; v = p[match[v]]) {
            blossom[base[v]] = blossom[base[match[v]]] = true;
            p[v] = children;
            children = match[v];
        }
    }

    static int findPath(List<List<Integer>> graph, int[] match, int[] p, int root) {
        int n = graph.size();
        boolean[] used = new boolean[n];
        Arrays.fill(p, -1);
        int[] base = new int[n];
        for (int i = 0; i < n; ++i)
            base[i] = i;

        used[root] = true;
        int qh = 0;
        int qt = 0;
        int[] q = new int[n];
        q[qt++] = root;
        while (qh < qt) {
            int v = q[qh++];

            for (int to : graph.get(v)) {
                if (base[v] == base[to] || match[v] == to) continue;
                if (to == root || match[to] != -1 && p[match[to]] != -1) {
                    int curbase = lca(match, base, p, v, to);
                    boolean[] blossom = new boolean[n];
                    markPath(match, base, blossom, p, v, curbase, to);
                    markPath(match, base, blossom, p, to, curbase, v);
                    for (int i = 0; i < n; ++i)
                        if (blossom[base[i]]) {
                            base[i] = curbase;
                            if (!used[i]) {
                                used[i] = true;
                                q[qt++] = i;
                            }
                        }
                } else if (p[to] == -1) {
                    p[to] = v;
                    if (match[to] == -1)
                        return to;
                    to = match[to];
                    used[to] = true;
                    q[qt++] = to;
                }
            }
        }
        return -1;
    }

    public static int maxMatching(List<List<Integer>> graph) {
        int n = graph.size();
        int[] match = new int[n];
        Arrays.fill(match, -1);
        int[] p = new int[n];
        for (int i = 0; i < n; ++i) {
            if (match[i] == -1) {
                int v = findPath(graph, match, p, i);
                while (v != -1) {
                    int pv = p[v];
                    int ppv = match[pv];
                    match[v] = pv;
                    match[pv] = v;
                    v = ppv;
                }
            }
        }

        int matches = 0;
        for (int i = 0; i < n; ++i)
            if (match[i] != -1)
                ++matches;
        return matches / 2;
    }

    // Usage example
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        List<String> people = new ArrayList<String>();
        scanner.nextLine();
        String line = scanner.nextLine();
        while (!line.equals("Connections: "))
        {
            people.add(line);
            line = scanner.nextLine();
        }
        Map<String, Integer> mask = new LinkedHashMap<String, Integer>();

        for (int i = 0; i < people.size(); i++)
        {
            mask.put(people.get(i), i);
        }

        List<List<Integer>> graph = new ArrayList<List<Integer>>(people.size());
        for (int i = 0; i < people.size(); i++)
        {
            graph.add(new ArrayList<Integer>());
        }
        line = scanner.nextLine();
        while (!line.equals("END"))
        {
            String[] pair = pattern.split(line);
            int parent = mask.get(pair[0]);
            int child = mask.get(pair[1]);
            graph.get(parent).add(child);
            graph.get(child).add(parent);
            line = scanner.nextLine();
        }

        System.out.println(maxMatching(graph));
    }
}
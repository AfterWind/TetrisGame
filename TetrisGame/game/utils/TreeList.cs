using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game.utils {
    public class TreeList<T> {

        private Dictionary<T, List<T>> nodes = new Dictionary<T, List<T>>();
        public T Root { get; private set; }

        public TreeList(T root) {
            this.nodes.Add(root, new List<T>());
            this.Root = root;
        }

        public void AddChild(T child, T parent) {
            if (!nodes.Keys.Contains(parent))
                nodes.Add(parent, new List<T>());
            nodes[parent].Add(child);
        }

        public T GetParent(T element) {
            foreach (T parent in nodes.Keys) {
                foreach (T child in nodes[parent]) {
                    if (child.Equals(element))
                        return parent;
                }
            }
            return default(T);
        }
    }
}

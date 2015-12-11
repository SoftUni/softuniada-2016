/*
TASK:square
LANG:C++
*/
#include <iostream>
#include <vector>
#include <queue>
using namespace std;

const int NMAX = 1024;

struct Point
{ int x,y; };

typedef Point Square[4];

Square S[NMAX];
vector<int> a[NMAX]; 
int d[NMAX];
int n;


void input()
{ cin >> n;
  for(int i=1; i<=n; i++)
  { int xM,yM;
    cin >> xM >> yM;
    cin >> S[i][0].x >> S[i][0].y;
    int p = S[i][0].x - xM;
    int q = S[i][0].y - yM;
    S[i][1].x = xM - q;
    S[i][1].y = yM + p;
    S[i][2].x = xM - p;
    S[i][2].y = yM - q;
    S[i][3].x = xM + q;
    S[i][3].y = yM - p;
  }
}    

int dir(Point P, Point Q, Point R)
{ int a = Q.x - P.x;
  int b = Q.y - P.y;
  int c = R.x - P.x;
  int d = R.y - P.y;
  if(a*d > b*c) return  1;
  if(a*d < b*c) return -1;
  return 0;
}

bool split(Point P, Point Q, Square A, Square B)
{ for(int i=0; i<4; i++)
  for(int j=0; j<4; j++)
    if(dir(P,Q,A[i])*dir(P,Q,B[j])>0) return false;
  return true;
}

bool intersect(Square A, Square B)
{ for(int i=0; i<4; i++)
  { if(split(A[i],A[(i+1)%4],A,B)) return false;  
    if(split(B[i],B[(i+1)%4],A,B)) return false;  
  }  
  return true;
}


void make_adj_lists()
{ for(int i=1; i<n; i++)
    for(int j=i+1; j<=n; j++)
      if(intersect(S[i],S[j]))
      { a[i].push_back(j);
        a[j].push_back(i);
      }
}
  
void bfs(int x)
{ for(int i=1; i<=n; i++)
    d[i] = -1;
  d[x] = 0;
  queue<int> q;
  q.push(x);
  
  while(!q.empty())
  { x = q.front(); 
    q.pop();
    
    for(int i=0; i<a[x].size(); i++)
    { int y = a[x][i];
      if(d[y]==-1)
      { d[y] = d[x] + 1;
        q.push(y);
      }
    }
  } 
}       

  
int main()
{ input();

  make_adj_lists();  
  
  bfs(1); 
  cout << d[n] << endl;
 
  return 0;
}    

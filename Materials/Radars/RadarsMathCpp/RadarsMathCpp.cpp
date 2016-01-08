// Time-stamp: </b1/wrf/c/2007/union-circle/uc.cc, Fri,  1 Jan 2010, 08:43:46 EST, http://wrfranklin.org/>

// Compute the Perimeter and Area of the union of circles of radius R;
// If there is a command line arg, then it is R.  Else R=1.
// Read (x,y) circle centers from cin.
// E.g. uc < protein.dat

// W. Randolph Franklin, Professor
// ECSE Dept., 6026 JEC,
// Rensselaer Polytechnic Inst,
// 110 8th St,
// Troy NY, 12180 USA
// 
// +1 (518) 276-6077
// 
// http://wrfranklin.org/
// email is on web page

// You may use my program for nonprofit research and education.  However,
// please credit me, e.g., in publications.

// The algorithm is very sensitive to roundoff error.  Small computation
// errors can cause large output errors, if they cause an intersection to
// be missed (or a false intersection to be found).  Therefore, I do not
// recommend it for large datasets, or for data with near degeneracies
// (circles that almost or barely intersect).

// One way to test this is to transform the data in a way that would change
// the roundoff errors but not the correct answer, and then rerun the
// program.  E.g., you might rotate the data.

//================================================================
// includes
//================================================================

#include <boost/array.hpp> 
#include <boost/progress.hpp>
#include <iomanip>
#include <iostream>
#include <sstream>
#include <math.h>
#include <vector>


//================================================================
// usings
//================================================================

using namespace std;
using boost::array;
using boost::timer;
using boost::progress_timer;
using boost::progress_display;


//================================================================
// macros
//================================================================

// Print an expression's name then its value, possible followed by a comma or endl.  
// Ex: cout << PRINTC(x) << PRINTN(y);

#define PRINT(arg)  #arg "= " << (arg)
#define PRINTC(arg)  redtty << #arg << deftty << "= " << (arg) << ", "
#define PRINTN(arg)  redtty << #arg << deftty << "= " << (arg)  << endl

// Execute a block of code then print the code and its time to cout.
// Ex:   RT(do_process()) ;

#define RT(arg) { boost::progress_timer bpt; {arg;}; cout << "Time to exec " << redtty << #arg << deftty << "is ";  }
// #define RT(arg) { arg;   }


// Compact for loop
// Ex:   FOR(i,0,3,a[i]=0);

#define FOR(foriterator,formin,formaxplus1,forbody) {for(int foriterator=formin; foriterator<formaxplus1; foriterator++) {forbody;};}


//================================================================
// typedefs
//================================================================

typedef boost::array<double, 2> Tpt;


//================================================================
// class routines
//================================================================

ostream &operator<<(ostream &o, const Tpt a) {
	o << "(" << a[0] << ", " << a[1] << ')';
	return o;
}

ostream &operator<<(ostream &o, const boost::array<Tpt, 2> a) {
	o << "(" << a[0] << ", " << a[1] << ')';
	return o;
}


inline const Tpt operator+(const Tpt &a, const Tpt b) {
	Tpt c(a);
	c[0] += b[0];
	c[1] += b[1];
	return c;
}

inline const Tpt operator-(const Tpt &a, const Tpt b) {
	Tpt c(a);
	c[0] -= b[0];
	c[1] -= b[1];
	return c;
}

template<typename T>
inline const Tpt operator*(const Tpt &a, const T r) {
	Tpt c(a);
	c[0] *= r;
	c[1] *= r;
	return c;
}


template<typename T>
inline const boost::array<T, 2> operator-(const boost::array<T, 2> a) {
	array<T, 2> b;
	b[0] = -a[0];
	b[1] = -a[1];
	return b;
}


template<typename T>   // Add a vector to the negative of each column of an array.
inline const boost::array<T, 2> operator-(const T &a, const boost::array<T, 2> b) {
	boost::array<T, 2> c(-b);
	c[0] += a;
	c[1] += a;
	return c;
}


inline const Tpt rot90(const Tpt &p) {
	Tpt q;
	q[0] = p[1];
	q[1] = -p[0];
	return q;
}


//================================================================
// variables
//================================================================

const string redtty("\033[2;31m");   // tell tty to switch to red
const string bluetty("\033[1;34m");   // tell tty to switch to blue
const string deftty("\033[0m");      // tell tty to switch back to default color
const int debug(0);
const int debugtime(0);

double R;  //  circle radius
double RR;
const double Pi(3.14159265358979323846264338327950288419716939937510);

int NCenters;
vector<Tpt> Centers;

int NXsects;
vector<Tpt> Xsects;

int NVisXsects;
int NVisRtPt;
int NVisLtPt;

double Perimeter;    // of the union.  Output value.
double Area;


//================================================================
// procedures
//================================================================


void ReadCenters() {
	Centers.clear();
	Tpt center;
	for (int i = 0;; i++) {
		cin >> center[0] >> center[1];
		if (!cin.good()) break;
		Centers.push_back(center);
		if (debug) cout << PRINTC(i) << PRINTN(Centers[i]);
	}
}


template<class T>
inline const T sqr(const T x) {
	return x*x;
}


inline const double distsq(const Tpt p, const Tpt q) {
	return sqr(p[0] - q[0]) + sqr(p[1] - q[1]);
}


inline const int DoCirclesIntersect(const int ic, const int jc,   // ids of the circles
	boost::array<Tpt, 2> &Intersections) {  // 2 returned intersections
	const double dd(distsq(Centers[ic], Centers[jc]));
	if (debug) cout << PRINTC(dd) << PRINTN(dd - 1);
	if (dd >= 4.0*RR) {
		if (debug) cout << "DoCirclesIntersect: " << PRINTC(ic) << PRINTC(jc) << " no\n";
		return 0;
	}
	const double a = sqrt(RR / dd - 0.25);
	const Tpt mid((Centers[ic] + Centers[jc])*0.5);
	const Tpt d(Centers[ic] - Centers[jc]);
	const Tpt drot90(rot90(d));

	Intersections[0] = mid + drot90 * a;
	Intersections[1] = mid - drot90 * a;

	if (debug)
		cout << PRINTC(Centers[ic]) << PRINTC(Centers[jc]) << PRINTC(a) << PRINTC(mid) << PRINTC(d) << PRINTN(drot90)
		<< PRINTC(distsq(Centers[ic], Intersections[0])) << PRINTC(distsq(Centers[ic], Intersections[1])) << PRINTC(distsq(Centers[jc], Intersections[0])) << PRINTN(distsq(Centers[jc], Intersections[1]))
		<< "DoCirclesIntersect:\n" << PRINTC(ic) << PRINTC(jc) << PRINTC(Intersections[0]) << PRINTN(Intersections[1]);
	return 1;
}


inline const int IsPointHidden(const int ic, const int jc, const Tpt p) {
	// Is point p hidden by any circle other than ic or jc?
	// ok for ic==jc
	for (int kc = 0; kc<NCenters; kc++) {
		if (kc == ic || kc == jc) continue;
		const double dd(distsq(p, Centers[kc]));
		if (debug) cout << "IsPointHidden: " << PRINTC(ic) << PRINTC(jc) << PRINTC(dd) << PRINTN(dd - 1);
		if (dd < RR) {
			if (debug) cout << p << ", an intersection of " << ic << "," << jc << " is hidden by " << kc << endl;
			return 1;
		}
	}
	if (debug) cout << p << ", an intersection of " << ic << ", " << jc << " is visible.\n";
	return 0;
}



inline const double atanp(const Tpt v) {
	return atan2(v[1], v[0]);
}


template <class T>
inline const int signum(const T x) {
	return x>0 ? 1 : x == 0 ? 0 : -1;
}


template <class T>    // Reduce an angle to the interval (-Pi,Pi].
inline const T norm1(const T x) {
	T y(x);
	if (y > Pi) y -= 2 * Pi;
	else if (y < -Pi) y += 2 * Pi;
	return y;
}


template <class T>    // Reduce an angle to the interval [0,2{i)
inline const T norm2(const T x) {
	T y(x);
	if (y >= 2 * Pi) y -= 2 * Pi;
	else if (y < 0) y += 2 * Pi;
	return y;
}


inline const double PartialIntersectionPerimeter(const int ic, const int jc, const Tpt Xsect) {
	if (debug) cout << PRINTC(distsq(Centers[ic], Xsect)) << PRINTN(distsq(Centers[jc], Xsect));

	// This little piece of code is the heart of the perimeter computation.  Enjoy.  :-)

	const double a1(atanp(Xsect - Centers[ic]));
	const double a2(atanp(Xsect - Centers[jc]));
	const double da1(norm1(a2 - a1));
	const double da2(norm2(a2) - norm2(a1));
	const double p(signum(da1)*da2);
	if (debug) cout << "PartialIntersectionPerimeter: " << PRINTC(ic) << PRINTC(jc) << PRINTC(Xsect) << PRINTC(a1) << PRINTC(a2) << PRINTC(da1) << PRINTC(da2) << PRINTN(p);
	return p;
}


// integral(sqrt(r^2-x^2) == x*sqrt(r*r-x*x)/2 + r*r*atan(x/sqrt(r*r-x*x))/2
// definite integral from a to r:
//cg = -sqrt(r * r - a * a) * a / 0.2e1 - r * r * asin(a / r) / 0.2e1 + r * r * 0.3141592654e1 / 0.4e1;

inline const double PartialIntersectionArea(const int ic, const int jc, const Tpt Xsect) {

	const boost::array<int, 2> c = { ic, jc };
	const boost::array<Tpt, 2>  p = { Xsect - Centers[ic], Xsect - Centers[jc] };

	if (debug) cout << bluetty << "PartialIntersectionArea: " << deftty << PRINTC(ic) << PRINTC(jc) << PRINTN(Xsect) << PRINTN(p);

	const boost::array<double, 2> theta = { norm1(atanp(p[0])), norm1(atanp(p[1])) };


	const double SecondPositive(norm1(theta[1] - theta[0]));

	if (debug) cout << PRINTC(theta) << PRINTN(SecondPositive);

	boost::array<double, 2> a;
	for (int i = 0; i<2; i++) {
		a[i] =
			-sqrt(RR - sqr(p[i][0]))*p[i][0] / 2
			- RR*asin(p[i][0] / R) / 2
			+ RR*(Pi / 4);
		if (theta[i]<0) a[i] = -a[i];
		a[i] += (R - p[i][0])*Centers[c[i]][1];
	}


	const double aa(signum(SecondPositive) * (a[1] - a[0]));

	if (debug) cout << PRINTC(a) << PRINTN(aa);

	return aa;
}


inline void FindAndProcessIntersections() {
	for (int ic = 0; ic<NCenters; ic++)
		for (int jc = ic + 1; jc<NCenters; jc++) {
			boost::array<Tpt, 2> Intersections;
			if (!DoCirclesIntersect(ic, jc, Intersections)) continue;
			NXsects += 2;
			for (int ii = 0; ii<2; ii++) {
				if (IsPointHidden(ic, jc, Intersections[ii])) continue;
				NVisXsects++;
				Area += PartialIntersectionArea(ic, jc, Intersections[ii]);
				Perimeter += PartialIntersectionPerimeter(ic, jc, Intersections[ii]);
			}
		}
}


inline const double PartialRtPtArea(const Tpt RtPt) {
	const double a(0);
	if (debug) cout << "PartialRtPtArea: " << PRINTN(a);
	return a;
}


inline const double PartialLtPtArea(const Tpt RtPt) {
	const double a(Pi*RR);
	if (debug) cout << "PartialLtPtArea: " << PRINTN(a);
	return a;
}


inline const double PartialRtPtPerimeter(const Tpt RtPt) {
	return 2 * Pi*R;
}


void ProcessExtremeXPts() {
	for (int ic = 0; ic<NCenters; ic++) {
		const Tpt p(Centers[ic]);
		Tpt RtPt(p);
		Tpt LtPt(p);
		RtPt[0] += R;     // This is the point on the right extreme of the circle.
		LtPt[0] -= R;
		if (!IsPointHidden(ic, ic, RtPt)) {
			NVisRtPt++;
			Area += PartialRtPtArea(p);
			Perimeter += PartialRtPtPerimeter(p);
		}
		if (!IsPointHidden(ic, ic, LtPt)) {
			NVisLtPt++;
			Area += PartialLtPtArea(p);
		}
	}
}


int main(int argc, char** argv) {
	boost::progress_timer t;  // start timing; value will print at end.
	if (argc >= 2) {
		istringstream stin(argv[1]);
		stin >> R;
	}
	else
		R = 1.0;
	RR = R*R;

	ReadCenters();
	NCenters = Centers.size();
	//cout << PRINTC(R) << PRINTN(NCenters);
	NXsects = 0;
	NVisXsects = 0;
	NVisRtPt = 0;
	NVisLtPt = 0;
	Area = 0.0;
	Perimeter = 0.0;
	FindAndProcessIntersections();
	ProcessExtremeXPts();

	cout << "Area = " << Area << endl;
	/*cout << PRINTC(NXsects) << PRINTC(NVisXsects) << PRINTC(NVisRtPt) << PRINTC(NVisLtPt)
		<< PRINTC(Area) << PRINTN(Perimeter);*/
	cout << "\nTotal time is ";  // following value will print  on exit
}


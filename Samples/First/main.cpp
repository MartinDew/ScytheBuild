#include <iostream>
#include <intrin.h>
using namespace std;

int main()
{
    // Try a intrinsic func for clang-cl
    int cpuInfo[4];
    __cpuid(cpuInfo, 1);
    
    cout << "Hello World!";
    return 0;
}
export module Test:Part;
import <iostream>;

export
class Partition
{
public:
    void Print() {
        std::cout << "Hello, World From the partition!" << std::endl;
    }
    
    void Soup();
};
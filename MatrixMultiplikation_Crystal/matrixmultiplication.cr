require "random"
require "benchmark"

module MatrixMultiplication
	
  @@r = Random.new

	def self.makeMatrix(size) : Array(Array(Float32))
  		arr = Array(Array(Float32)).new
    	size.times do |x|
   	    	row = Array(Float32).new
      		size.times do |y|
    	    	row << @@r.next_float.to_f32
      		end
      		arr << row
    	end
  		arr
 	end
  
  def self.multiply(m1, m2, arr)
    m2Width = m2[0].size
    row = Array(Float32).new(m2Width,0)
    total = uninitialized Float32
    m1.size.times do |x| # zeile in m1
      m2Width.times do |y|# spalte in m2
        total = 0.0
        m2.size.times do |z|# wert in der zeile von m1
          total += m1[x][z] * m2[z][y]
        end
        row[y] = total
      end
      arr[x] = row
    end
    arr
  end

  
end

SIZE = 1440
THREADS = 24

m1 = MatrixMultiplication.makeMatrix(SIZE)
m2 = MatrixMultiplication.makeMatrix(SIZE)
arr = Array(Array(Float32)).new(m1.size, Array(Float32).new)

puts Benchmark.realtime {
  m3 = MatrixMultiplication.multiply(m1, m2, arr)
}

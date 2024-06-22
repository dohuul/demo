import { group, check } from 'k6';
import http from 'k6/http';

export const options = {
    vus: 1,
    iteration: 1
}

  
const id = 5;


  
export default function () {

  // reconsider this type of code
  group('get post', function () {
    http.get(`http://example.com/posts/${id}`);
  });
  group('list posts', function () {
    const res = http.get(`http://example.com/posts`);
    
  });
  
  
  }


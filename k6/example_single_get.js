import http from 'k6/http'

export const options = {
  vus: 1,
  iteration: 1,
  duration: 1,
  stages: []
}

//define script parameters: environment, number of threads, test type=light or heavy, duration, spike test 

//define option, threads, duration, spike test

//prepare test data - load from csv file



export default function () {

    let url = '';
    let data = '';

}
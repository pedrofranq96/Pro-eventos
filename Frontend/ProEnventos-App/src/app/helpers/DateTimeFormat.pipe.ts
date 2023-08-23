import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { Constants } from '../util/constants';

@Pipe({
  name: 'DateFormatPipe'
})
export class DateTimeFormatPipe extends DatePipe implements PipeTransform {

  transform(value: any): any {
    let newValue = value
    if(value && !(value instanceof Date)){
      let month = value.substring(0,2)
      let day = value.substring(3,5)
      let year = value.substring(6,10)
      let hour = value.substring(11,13)
      let minutes = value.substring(14,16)

      newValue = day + '/' + month + '/' + year + '/' + hour + ':' + minutes
    }
    return super.transform(newValue, Constants.DATE_TIME_FMT);
  }

  // transform(value: any): any {
  //   if (value && !(value instanceof Date)) {
  //     // Assuming the input format is 'dd/MM/yyyy HH:mm:ss'
  //     const parts = value.split(' ');
  //     const dateParts = parts[0].split('/');
  //     const timeParts = parts[1].split(':');

  //     const year = parseInt(dateParts[2], 10);
  //     const month = parseInt(dateParts[1], 10) - 1; // Months are 0-based
  //     const day = parseInt(dateParts[0], 10);
  //     const hour = parseInt(timeParts[0], 10);
  //     const minutes = parseInt(timeParts[1], 10);

  //     const transformedDate = new Date(year, month, day, hour, minutes);
  //     const datePipe = new DatePipe('en-US'); // You can change the locale as needed
  //     return datePipe.transform(transformedDate, Constants.DATE_TIME_FMT);
  //   }

  //   return value; // Return the value as-is if it's already a Date
  // }

}

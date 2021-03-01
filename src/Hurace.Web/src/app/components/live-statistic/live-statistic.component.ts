import { Component, Input } from '@angular/core';
import { LiveStatisticDto, LiveRaceDataDto } from 'src/app/dtos';
import { Props } from 'src/app/models';

@Component({
    selector: 'app-live-statistic',
    templateUrl: './live-statistic.component.html',
    styleUrls: ['./live-statistic.component.scss']
})
export class LiveStatisticComponent {
    @Input() public statistic: LiveStatisticDto = null;
    public displayedColumns: Props<LiveRaceDataDto> = ['sensorId', 'totalTime', 'timeStamp'];
}

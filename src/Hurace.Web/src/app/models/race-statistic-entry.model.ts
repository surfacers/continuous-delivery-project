export interface RaceStatisticEntry {
    currentPosition: number;
    deltaPosition: number | null;
    skierId: number;
    skierName: string;
    skierCountry: string;
    time: string;
    deltaTimeLeadership: string;
}

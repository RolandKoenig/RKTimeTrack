/**
 * Returns the current date in current timezone in the format yyyy-mm-dd
 */
export function getCurrentDateAsString(): string{
    const currentDate = new Date();
    const offset = currentDate.getTimezoneOffset()
    return new Date(currentDate.getTime() - (offset*60*1000))
        .toISOString()
        .split('T')
        [0] ?? "";
}